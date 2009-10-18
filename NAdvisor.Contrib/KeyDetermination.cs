using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NAdvisor.Contrib
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
