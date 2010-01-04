using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Interceptor;
using System.Reflection;

namespace NAdvisor.Core
{
    internal class AdviceInterceptor : IInterceptor
    {
        private readonly Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> _getAspects;
        private readonly IKeyValueStore _keyValueStore;
        private readonly IList<IAspect> _availableAspects;
        private readonly object _concreteInstance;
        private readonly Type _interfaceType;

        //used for caching the aspectEnvironment
        private readonly Dictionary<MethodInfo, AspectEnvironment> _aspectEnvironmentDict;
        private readonly object _aspectEnvironmentSyncRott = new object();


        public AdviceInterceptor(Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspects, IKeyValueStore store, IEnumerable<IAspect> availableAspects, object concreteInstance, Type interfaceType)
        {
            _availableAspects = new List<IAspect>(availableAspects);
            _getAspects = getAspects;
            _keyValueStore = store;
            _concreteInstance = concreteInstance;
            _interfaceType = interfaceType;
            _aspectEnvironmentDict = new Dictionary<MethodInfo, AspectEnvironment>();
        }

        public void Intercept(IInvocation invocation)
        {
            IAspectEnvironment environment = CreateOrGetEnvironment(invocation);
            var aspectList = _getAspects.Invoke(environment, new List<IAspect>(_availableAspects));
            invocation.ReturnValue = CreateNextLevel(invocation, aspectList, environment)(invocation.Arguments);
        }

        private AspectEnvironment CreateOrGetEnvironment(IInvocation invocation)
        {
            lock (_aspectEnvironmentSyncRott)
            {
                var calledMethod = invocation.Method;

                if (_aspectEnvironmentDict.ContainsKey(calledMethod))
                    return _aspectEnvironmentDict[calledMethod];
                else
                {
                    AspectEnvironment environment = CreateEnvironment(invocation);
                    _aspectEnvironmentDict.Add(calledMethod, environment);

                    return environment;
                }
            }
        }

        private AspectEnvironment CreateEnvironment(IInvocation invocation)
        {
            var environment = new AspectEnvironment(_keyValueStore);

            environment.ConcreteMethodInfo = invocation.GetConcreteMethod();
            environment.ConcreteObject = _concreteInstance;

            environment.InterfaceMethodInfo = invocation.Method;

            environment.ConcretetType = _concreteInstance.GetType();
            environment.InterfaceType = _interfaceType;
            return environment;
        }

        public Func<object[], object> CreateNextLevel(IInvocation invocation, IEnumerable<IAspect> restOfAspects, IAspectEnvironment method)
        {
            return args =>
                       {
                           IAspect nextAspect = restOfAspects.FirstOrDefault();
                           if (nextAspect != null)
                           {
                               //Recursion
                               return nextAspect.Execute(
                                   CreateNextLevel(invocation, restOfAspects.Skip(1), method), 
                                   args, method
                                   );
                           }

                           for (int i = 0; i < args.Length; i++)
                           {
                               invocation.SetArgumentValue(i, args[i]);
                           }
                           //Enter the body of the concrete Method
                           invocation.Proceed();
                           return invocation.ReturnValue;
                       };
        }
    }
}