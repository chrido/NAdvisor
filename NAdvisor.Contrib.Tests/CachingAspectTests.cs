using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace NAdvisor.Contrib.Tests
{
    [TestFixture]
    public class CachingAspectTests
    {
        [Test]
        public void TestingMethodInvoke()
        {
            var cachingAspect = new CachingAspect();
            var cachedClass = new MethodInvokesShouldBeCachedClass();

            var advisor = new Advisor(cachingAspect);
            var advicedProxy = advisor.GetAdvicedProxy(cachedClass);

            int result = advicedProxy.AwfulLongComputation("hello");
        }
    }

    public interface IMethodInvokesShouldBeCachedClass
    {
        int AwfulLongComputation(string inputParameter);
    }

    public class MethodInvokesShouldBeCachedClass : IMethodInvokesShouldBeCachedClass
    {
        
        public int AwfulLongComputation(string inputParameter)
        {
            int count = 0;

            foreach (char c in inputParameter.ToCharArray())
            {
                count += (int) c;
            }

            return count;
        }
    }
}
