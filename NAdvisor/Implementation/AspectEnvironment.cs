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

using System.Reflection;
using System.Collections.Generic;

namespace NAdvisor.Core
{
    internal class AspectEnvironment : IAspectEnvironment
    {
        private readonly Dictionary<string, object> _proxyDict;
        private readonly object _syncRoot = new object();

        public AspectEnvironment()
        {
            _proxyDict = new Dictionary<string, object>();
        }


        public MethodInfo ConcreteMethodInfo { get; set; }
        public MethodInfo InterfaceMethodInfo { get; set; }
        public object ConcreteObject { get; set; }
        
        public T TryGetValue<T>(string key)
        {
            lock (_syncRoot)
            {

                if (_proxyDict.ContainsKey(key))
                {
                    var value = _proxyDict[key];
                    return value is T ? (T) value : default(T);
                }

                return default(T);
            }
        }

        public void SetValue(string key, object value)
        {
            lock (_syncRoot)
            {
                _proxyDict[key] = value;
            }
        }
    }
}