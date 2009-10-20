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
using Castle.DynamicProxy;

namespace NAdvisor.Core
{
    public class Advisor
    {
        private readonly Func<Type, IAspectEnvironment, IList<IAspect>, IList<IAspect>> _getAspectsForJointPoint;
        private readonly IList<IAspect> _aspects;

        public Advisor(IAspect aspect)
        {
            _aspects = new List<IAspect>(){aspect};
            _getAspectsForJointPoint = (type, methodInfo, availableAspects) => _aspects;
        }

        public Advisor(IList<IAspect> aspects)
        {
            _aspects = aspects;
            _getAspectsForJointPoint = (type, methodInfo, availableAspects) => _aspects;
        }

        public Advisor(Func<Type, IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspectsForJointPoint, IAspect aspect)
        {
            _getAspectsForJointPoint = getAspectsForJointPoint;
            _aspects = new List<IAspect>() {aspect};
        }

        public Advisor(Func<Type, IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspectsForJointPoint, IList<IAspect> aspects)
        {
            _getAspectsForJointPoint = getAspectsForJointPoint;
            _aspects = aspects;
        }

        public T GetAdvicedProxy<T>(T concreateInstance)
        {
            var proxyGenerator = new ProxyGenerator(new PersistentProxyBuilder());
            var aspectInterceptor = new AdviceInterceptor(_getAspectsForJointPoint, _aspects, concreateInstance);
            return (T)proxyGenerator.CreateInterfaceProxyWithTargetInterface(typeof(T), concreateInstance, aspectInterceptor); ;
        }
    }
}