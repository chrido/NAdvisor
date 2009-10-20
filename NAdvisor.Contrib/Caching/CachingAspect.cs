using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAdvisor.Core;

namespace NAdvisor.Contrib
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
