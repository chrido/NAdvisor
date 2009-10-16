using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NAdvisor.Contrib
{
    public class CachingConfig<TCachedInterface>
    {
        private readonly Dictionary<MethodInfo, object> _collectedCachingConfigsForInterface;

        public CachingConfig()
        {
            _collectedCachingConfigsForInterface = new Dictionary<MethodInfo, object>();
        }


        public CachingKeyCreator<TCachingKey> CreateKey<TCachingKey>()
        {


            return new CachingKeyCreator<TCachingKey>();
        }

        public CachingConfig<TCachedInterface> CreateCachingKey(Expression<Action<TCachedInterface>> methodExpression)
        {
            LambdaExpression expression = methodExpression as LambdaExpression;

            var methodCall = expression.Body as MethodCallExpression;
            if (methodCall == null)
                throw new Exception("needs a Methodcall on Interface as parameter");

            if(_collectedCachingConfigsForInterface.ContainsKey(methodCall.Method))
                throw new Exception("Caching behaviour of a Method can't be defined twice");

            _collectedCachingConfigsForInterface.Add(methodCall.Method, new object());

            return this;
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
