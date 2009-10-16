using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NAdvisor.Contrib.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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
                count += (int)c;
            }

            return count;
        }
    }

}



