using System;
using Castle.Core.Interceptor;

namespace NAdvisor.Contrib.Caching
{
    internal class FluentProxy : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Type returnType = invocation.Method.ReturnType;
        }
    }
}