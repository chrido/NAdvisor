using System;
using NAdvisor.Core;

namespace NAdvisor.Contrib.Caching
{
    public class CachingAspect : IAspect
    {
        private string cachingKey = string.Empty;

        public object Execute(Func<object[], object> proceedInvocation, object[] methodArguments, IAspectEnvironment method)
        {
            return proceedInvocation(methodArguments);
        }
    }
}