using System;
using System.Collections.Generic;
using System.Reflection;
using TechTalk.NAdvisor.Core;

namespace TechTalk.NAdvisor.Contrib
{
    public class MemoizeJointPointDefinition
    {
        private readonly Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> _jointPointDefinitionToMemoize;

        public MemoizeJointPointDefinition(Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> jointPointDefinitionToMemoize)
        {
            _jointPointDefinitionToMemoize = jointPointDefinitionToMemoize;
        }

        public Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> GetMemoizedJointPointDefinition()
        {
            return Memoization;
        }

        /// <summary>
        /// TODO using the additional Dictionary/MethodInfo thing is not that good idea, refactor!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="aspectEnvironment"></param>
        /// <param name="availableAspects"></param>
        /// <returns></returns>
        private IList<IAspect> Memoization(IAspectEnvironment aspectEnvironment, IList<IAspect> availableAspects)
        {
            const string dictionaryKey = "__MemoizeJointPointDefinition";

            IList<IAspect> methodAspect = null;

            aspectEnvironment.SyncronizedKeyValueStoreMutator((keyValueStore) =>
            {
                if (keyValueStore.ContainsKey(dictionaryKey))
                {
                    var methodInfoDict = keyValueStore.GetValue<Dictionary<MethodInfo, IList<IAspect>>>(dictionaryKey);

                    if (methodInfoDict != null && methodInfoDict.ContainsKey(aspectEnvironment.ConcreteMethodInfo))
                    {
                        methodAspect = methodInfoDict[aspectEnvironment.ConcreteMethodInfo];
                    }
                }
            });

            if (methodAspect != null)
                return methodAspect;

            aspectEnvironment.GetValueOrCreate(dictionaryKey, () => new Dictionary<MethodInfo, IList<IAspect>>());

            aspectEnvironment.SyncronizedKeyValueStoreMutator((keyValueStore) =>
            {
                var store = keyValueStore.GetValue<Dictionary<MethodInfo, IList<IAspect>>>(dictionaryKey);

                if (!store.ContainsKey(aspectEnvironment.ConcreteMethodInfo)) //second check
                {
                    methodAspect = _jointPointDefinitionToMemoize(aspectEnvironment, availableAspects);
                    store.Add(aspectEnvironment.ConcreteMethodInfo, methodAspect);
                }
            });

            return methodAspect;
        }

    }
}
