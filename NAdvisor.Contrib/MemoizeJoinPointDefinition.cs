﻿using System;
using System.Collections.Generic;
using System.Reflection;
using NAdvisor.Core;

namespace NAdvisor.Contrib
{
    public class MemoizeJoinPointDefinition
    {
        private readonly Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> _joinPointDefinitionToMemoize;

        public MemoizeJoinPointDefinition(Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> joinPointDefinitionToMemoize)
        {
            _joinPointDefinitionToMemoize = joinPointDefinitionToMemoize;
        }

        public Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> GetMemoizedJoinPointDefinition()
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
            const string dictionaryKey = "__MemoizeJoinPointDefinition";

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
                    methodAspect = _joinPointDefinitionToMemoize(aspectEnvironment, availableAspects);
                    store.Add(aspectEnvironment.ConcreteMethodInfo, methodAspect);
                }
            });

            return methodAspect;
        }

    }
}
