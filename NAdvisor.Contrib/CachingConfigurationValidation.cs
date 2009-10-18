using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NAdvisor.Contrib
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
