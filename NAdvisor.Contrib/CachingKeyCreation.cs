using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NAdvisor.Contrib
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
