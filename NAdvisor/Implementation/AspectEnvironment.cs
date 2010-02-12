using System;
using System.Reflection;
using System.Collections.Generic;
using TechTalk.NAdvisor.Core.Implementation;

namespace TechTalk.NAdvisor.Core
{
    internal class AspectEnvironment : IAspectEnvironment
    {
        private readonly object _syncRoot = new object();
        private readonly IKeyValueStore _dictKVStore;

        public AspectEnvironment(IKeyValueStore keyValueStore)
        {
            _dictKVStore = keyValueStore;
        }


        public MethodInfo ConcreteMethodInfo { get; set; }
        public MethodInfo InterfaceMethodInfo { get; set; }
        public object ConcreteObject { get; set; }
        public Type ConcretetType { get; set; }
        public Type InterfaceType { get; set; }

        public T TryGetValue<T>(string key)
        {
            lock (_syncRoot)
            {
                return _dictKVStore.TryGetValue<T>(key);
            }
        }

        public void SetValue(string key, object value)
        {
            lock (_syncRoot)
            {
                _dictKVStore.SetValue(key, value);
            }
        }

        public void SyncronizedKeyValueStoreMutator(Action<IKeyValueStore> action)
        {
            lock (_syncRoot)
            {
                action(_dictKVStore);
            }
        }

        public object GetValueOrCreate(string key, Func<object> getOrCreateFunc)
        {
            lock (_syncRoot)
            {
                object value = getOrCreateFunc();
                _dictKVStore.SetValue(key, value);
                return value;
            }
        }
    }
}