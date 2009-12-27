using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NAdvisor.Contrib.Caching
{
    public partial class CachingConfig<TCachedInterface>
    {
        private readonly Dictionary<MethodInfo, List<KeyDetermination>> _cachingConfigsForFunction;
        private int _createKeyCounter;
        private int _maxArgumentsCounter;

        public CachingConfig()
        {
            _cachingConfigsForFunction = new Dictionary<MethodInfo, List<KeyDetermination>>();
            _createKeyCounter = 0;
            _maxArgumentsCounter = 0;
        }


        public CachingKeyCreator<TCachingKey> CreateKey<TCachingKey>()
        {


            return new CachingKeyCreator<TCachingKey>();
        }

        public CachingConfig<TCachedInterface> CreateCachingKey(Expression<Action<TCachedInterface>> methodExpression)
        {
            _createKeyCounter = 0;
            _maxArgumentsCounter = 0;
            var expression = methodExpression as LambdaExpression;

            var methodCall = expression.Body as MethodCallExpression;
            if (methodCall == null)
                throw new ArgumentException("needs a Methodcall on Interface as parameter");

            if(_cachingConfigsForFunction.ContainsKey(methodCall.Method))
                throw new ArgumentException("Caching behaviour of a Method can't be defined twice");

            int argumentCount = methodCall.Arguments.Count;
            _cachingConfigsForFunction.Add(methodCall.Method, CreateCacheConfigs(methodCall, methodExpression));

            return this;
        }

        private List<KeyDetermination> CreateCacheConfigs(MethodCallExpression expression, Expression<Action<TCachedInterface>> methodExpression)
        {
            var keyDeterminationList = new List<KeyDetermination>();

            foreach (Expression argument in expression.Arguments)
            {
                keyDeterminationList.Add(new KeyDetermination(){InputType = argument.Type, MethodExpression = methodExpression});
            }

            return keyDeterminationList;
        }
    }
}