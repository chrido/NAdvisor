using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Interceptor;

namespace NAdvisor.Core
{
    internal class AdviceInterceptor : IInterceptor
    {
        private readonly Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> _getAspects;
        private readonly IKeyValueStore _keyValueStore;
        private readonly IList<IAspect> _availableAspects;
        private readonly object _concreteInstance;

        public AdviceInterceptor(Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspects, IKeyValueStore store, IEnumerable<IAspect> availableAspects, object concreteInstance)
        {
            _availableAspects = new List<IAspect>(availableAspects);
            _getAspects = getAspects;
            _keyValueStore = store;
            _concreteInstance = concreteInstance;
        }


        public void Intercept(IInvocation invocation)
        {
            IAspectEnvironment environment = CreateEnvironment(invocation);
            var aspectList = _getAspects.Invoke(environment, new List<IAspect>(_availableAspects));
            invocation.ReturnValue = CreateNextLevel(invocation, aspectList, environment)(invocation.Arguments);
        }

        private AspectEnvironment CreateEnvironment(IInvocation invocation)
        {
            var environment = new AspectEnvironment(_keyValueStore);
            environment.ConcreteMethodInfo = invocation.GetConcreteMethod();
            environment.ConcreteObject = _concreteInstance;
            environment.InterfaceMethodInfo = invocation.Method;

            environment.ConcretetType = _concreteInstance.GetType();
            environment.InterfaceType = invocation.TargetType;

            return environment;
        }

        public Func<object[], object> CreateNextLevel(IInvocation invocation, IEnumerable<IAspect> restOfAspects, IAspectEnvironment method)
        {
            return args =>
                       {
                           var nextAspect = restOfAspects.FirstOrDefault();
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
                           //Enter the concrete Method
                           invocation.Proceed();
                           return invocation.ReturnValue;
                       };
        }
    }
}