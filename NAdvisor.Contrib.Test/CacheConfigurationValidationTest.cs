using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAdvisor.Contrib.Caching;

namespace NAdvisor.Contrib.Test
{
    [TestClass]
    public class CacheConfigurationValidationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void caching_behavoior_cant_be_defined_twice()
        {
            var cc = new CachingConfig<IMethodInvokesShouldBeCachedClass>();
            cc
                .CreateCachingKey(
                    cck => cck.AwfulLongCacheAbleComputation(
                        cc.CreateKey<int>().From<string>(value => value.GetHashCode()
                        )
                    )
                )
                .CreateCachingKey(
                    cck => cck.AwfulLongCacheAbleComputation(
                        cc.CreateKey<int>().From<string>(value => value.GetHashCode()
                        )
                    )
                );
        }
    }
}
