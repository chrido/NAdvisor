using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using NAdvisor.Core.Implementation;

namespace NAdvisor.Core
{
    public class Advisor
    {
        private readonly Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> _getAspectsForJointPoint;
        private readonly IList<IAspect> _aspects;

        /// <summary>
        /// Intercepts every call with one aspect
        /// </summary>
        /// <param name="aspect"></param>
        public Advisor(IAspect aspect)
        {
            _aspects = new List<IAspect>(){aspect};
            _getAspectsForJointPoint = (aspectEnvironment, availableAspects) => _aspects;
        }

        /// <summary>
        /// Intercepts ever call with all espects, 
        /// </summary>
        /// <param name="aspects"></param>
        public Advisor(IList<IAspect> aspects)
        {
            _aspects = aspects;
            _getAspectsForJointPoint = (aspectEnvironment, availableAspects) => _aspects;
        }

        public Advisor(Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspectsForJointPoint, IAspect aspect)
        {
            _getAspectsForJointPoint = getAspectsForJointPoint;
            _aspects = new List<IAspect>() {aspect};
        }

        public Advisor(Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> getAspectsForJointPoint, IList<IAspect> aspects)
        {
            _getAspectsForJointPoint = getAspectsForJointPoint;
            _aspects = aspects;
        }


        public T GetAdvicedProxy<T>(T concreteInstance)
            where T : class
        {
            return GetAdvicedProxy(typeof(T), concreteInstance, new DictionaryKeyValueStore()) as T;
        }

        public object GetAdvicedProxy(Type interfaceType, object concreteInstance)
        {
            return GetAdvicedProxy(interfaceType, concreteInstance, new DictionaryKeyValueStore());
        }

        public T GetAdvicedProxy<T>(T concreteInstance, IKeyValueStore keyValueStore)
            where T : class 
        {
            return GetAdvicedProxy(typeof (T), concreteInstance, keyValueStore) as T;
        }

        public object GetAdvicedProxy(Type interfaceType, object concreteInstance, IKeyValueStore keyValueStore)
        {
            if (!interfaceType.IsInterface)
                throw new ArgumentException("need an interface to proxy");
            
            var proxyGenerator = new ProxyGenerator(new PersistentProxyBuilder());
            var aspectInterceptor = new AdviceInterceptor(_getAspectsForJointPoint, keyValueStore, _aspects, concreteInstance, interfaceType);
            return proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceType, concreteInstance, aspectInterceptor); ;
        } 
    }
}