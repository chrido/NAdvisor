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

namespace NAdvisor.Core
{
    public interface IKeyValueStore
    {
        void SetValue(string key, object value);

        /// <summary>
        /// Behaviour: does not throw an exception
        /// 
        /// if not found the default value is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T TryGetValue<T>(string key);

        /// <summary>
        /// Behaviour
        /// 
        /// Throws exception if key is not found and does not implements the fiven type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key);
    }
}