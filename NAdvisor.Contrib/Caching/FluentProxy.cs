﻿//   Copyright 2009 Christoph Doblander
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