using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAdvisor.Contrib
{
    public class CachingConfig<TCachedInterface>
    {
        public CachingKeyCreator<TCachingKey> CreateKey<TCachingKey>()
        {
            return new CachingKeyCreator<TCachingKey>();
        }

        public void CreateCachingKey(Action<TCachedInterface> methodToCache)
        {
        }

        public class CachingKeyCreator<TChachingKey>
        {
            public TFromClass From<TFromClass>(Func<TFromClass, TChachingKey> functionToCreateKey)
            {
                return default(TFromClass);
            }
        }

    }
}
