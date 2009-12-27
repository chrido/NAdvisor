using System;
using System.Linq.Expressions;

namespace NAdvisor.Contrib.Caching
{
    public partial class CachingConfig<TCachedInterface>
    {
        public class CachingKeyCreator<TChachingKey>
        {
            public TFromClass From<TFromClass>(Expression<Func<TFromClass, TChachingKey>> functionToCreateKey)
            {
                //TODO add functions for caching

                return default(TFromClass);
            }
        }

    }
}