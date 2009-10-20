using System;
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

using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace NAdvisor.Contrib.Caching
{
    public partial class CachingConfig<TCachedInterface>
    {
        public void BuildCachingConfiguration()
        {
            foreach (var cachingConfigForFunction in _cachingConfigsForFunction)
            {
                CheckConfig(cachingConfigForFunction.Key, cachingConfigForFunction.Value);
            }
        }

        private void CheckConfig(MethodInfo info, List<KeyDetermination> determinations)
        {
            CheckIfKeyDeterminationsEqualMethodInfo(info, determinations);

            foreach (var list in determinations)
            {
                Action<TCachedInterface> compile = list.MethodExpression.Compile();
                var proxyGenerator = new ProxyGenerator(new PersistentProxyBuilder());
                var proxy = (TCachedInterface) proxyGenerator.CreateInterfaceProxyWithoutTarget(typeof (TCachedInterface), new FluentProxy());

                compile.Invoke(proxy);



//                compile.Invoke();
            }
        }

        private void CheckIfKeyDeterminationsEqualMethodInfo(MethodInfo info, List<KeyDetermination> determinations)
        {
            var count = 0;

            foreach (ParameterInfo parameter in info.GetParameters())
            {
                if (parameter.IsOut)
                    throw new Exception("No out parameters allowed");

                if (parameter.ParameterType != determinations[count].InputType)
                    throw new Exception("Parameters in KeyDetermination do not match");

                count++;
            }
        }
    }
}