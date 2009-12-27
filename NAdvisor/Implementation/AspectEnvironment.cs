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
using System.Reflection;
using System.Collections.Generic;
using NAdvisor.Core.Implementation;

namespace NAdvisor.Core
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