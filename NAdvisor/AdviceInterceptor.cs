//   Copyright 2009 Christoph Doblander
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Interceptor;

namespace NAdvisor.Core
{
    internal class AdviceInterceptor : IInterceptor
    {
        private readonly Func<Type, IAspectEnvironment, IList<IAspect>, IList<IAspect>> _getAspects;
        private readonly IList<IAspect> _availableAspects;
        private readonly object _concreteInstance;

        public AdviceInterceptor(Func<Type, IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspects, IEnumerable<IAspect> availableAspects, object concreteInstance)
        {
            _availableAspects = new List<IAspect>(availableAspects);
            _getAspects = getAspects;
            _concreteInstance = concreteInstance;
        }

        public void Intercept(IInvocation invocation)
        {
            IAspectEnvironment environment = CreateEnvironment(invocation);
            var aspectList = _getAspects.Invoke(invocation.InvocationTarget.GetType(), environment, new List<IAspect>(_availableAspects));
            invocation.ReturnValue = CreateNextLevel(invocation, aspectList, environment)(invocation.Arguments);
        }

        private AspectEnvironment CreateEnvironment(IInvocation invocation)
        {
            var environment = new AspectEnvironment();
            environment.ConcreteMethodInfo = GetMethodInfoFromConcreteType(invocation.Method, _concreteInstance);
            environment.ConcreteObject = _concreteInstance;
            environment.InterfaceMethodInfo = invocation.Method;
            
            return environment;
        }

        /// <summary>
        /// Based on the given <paramref name="methodInfo"/> the same Method on the concreate Implementation is searched
        /// Because the concreteInstance must have this Method since it implements the interface theire is no error handling necessary
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="concreteInstance"></param>
        /// <returns></returns>
        private MethodInfo GetMethodInfoFromConcreteType(MethodInfo methodInfo, object concreteInstance)
        {
            //Could be cached ...

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            Type[] allTypes = new Type[parameterInfos.Length];
            for(int i = 0; i < allTypes.Length; i++)
                allTypes[i] = parameterInfos[i].ParameterType;

            return concreteInstance.GetType().GetMethod(methodInfo.Name, allTypes);
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