using System;

namespace NAdvisor.Core
{
    public interface IAspect
    {
        object Execute(Func<object[], object> proceedInvocation, object[] methodArguments, IAspectEnvironment method);
    }
}