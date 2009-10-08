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

namespace NAdvisor
{
    public interface IAspectEnvironment
    {
        /// <summary>
        /// Method Info of the Concrete Implementation
        /// </summary>
        MethodInfo ConcreteMethodInfo { get; }

        /// <summary>
        /// Method Info of the Interface Method
        /// </summary>
        MethodInfo InterfaceMethodInfo { get;}

        /// <summary>
        /// Concrete Object which implements the interface which is proxied
        /// </summary>
        object ConcreteObject { get; }

        T TryGetValue<T>(string key);

        void SetValue(string key, object value);
    }

    internal class AspectEnvironment : IAspectEnvironment
    {
        private readonly Dictionary<string, object> _proxyDict;
        private readonly object syncRoot;

        public AspectEnvironment()
        {
            _proxyDict = new Dictionary<string, object>();
        }


        public MethodInfo ConcreteMethodInfo { get; set; }
        public MethodInfo InterfaceMethodInfo { get; set; }
        public object ConcreteObject { get; set; }
        
        public T TryGetValue<T>(string key)
        {
            lock (syncRoot)
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
            lock (syncRoot)
            {
                _proxyDict[key] = value;
            }
        }
    }
}
