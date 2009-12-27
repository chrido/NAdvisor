using System;
using System.Linq.Expressions;

namespace NAdvisor.Contrib.Caching
{
    public partial class CachingConfig<TCachedInterface>
    {
        public class KeyDetermination
        {
            public Type InputType { get; set; }
            public Type KeyType { get; set; }
            public object CalculationFunction { get; set; }

            public Expression<Action<TCachedInterface>> MethodExpression { get; set; }
        }
    }
}