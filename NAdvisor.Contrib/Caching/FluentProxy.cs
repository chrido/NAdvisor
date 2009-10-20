using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;

namespace NAdvisor.Contrib
{
    internal class FluentProxy : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Type returnType = invocation.Method.ReturnType;
        }
    }
}
