﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NAdvisor.Contrib.Test
{
    /// <summary>
    /// Summary description for CachingAspectTest
    /// </summary>
    [TestClass]
    public class CachingAspectTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cc = new CachingConfig<IMethodInvokesShouldBeCachedClass>();
            cc
                .CreateCachingKey(
                    cck => cck.AwfulLongCacheAbleComputation(
                        cc.CreateKey<int>().From<string>(value => value.GetHashCode()
                        )
                    )
                );

            var cachingAspect = new CachingAspect();
            var cachedClass = new MethodInvokesShouldBeCachedClass();

            var advisor = new Advisor(cachingAspect);
            var advicedProxy = advisor.GetAdvicedProxy<IMethodInvokesShouldBeCachedClass>(cachedClass);

            int result = advicedProxy.AwfulLongCacheAbleComputation("hello");
        }

        [TestMethod]
        public void Should_Create_Test_config_for_testing_a_few_methodcalls()
        {
            var cc = new CachingConfig<IMethodInvokesShouldBeCachedClass>();
            cc
                .CreateCachingKey(
                cck => cck.AwfulLongCacheAbleComputation(
                           cc.CreateKey<int>().From<string>(value => value.GetHashCode()
                               )
                           )
                );

            cc.BuildCachingConfiguration();
        }
    }
}



