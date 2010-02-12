using System.Collections.Generic;

namespace TechTalk.NAdvisor.Core.Implementation
{
    internal class DictionaryKeyValueStore : IKeyValueStore
    {
        private readonly Dictionary<string, object> _dict;

        public DictionaryKeyValueStore()
        {
            _dict = new Dictionary<string, object>();
        }

        public void SetValue(string key, object value)
        {
            _dict[key] = value;
        }

        public T TryGetValue<T>(string key)
        {
            if (!_dict.ContainsKey(key))
                return default(T);

            object foundKey = _dict[key];
            return foundKey is T ? (T) foundKey : default(T);
        }

        public T GetValue<T>(string key)
        {
            if (!_dict.ContainsKey(key))
                throw new KeyNotFoundException("key not found in dictionary");

            object foundKey = _dict[key];
            return foundKey is T ? (T)foundKey : default(T);
        }

        public bool ContainsKey(string dictionaryKey)
        {
            return _dict.ContainsKey(dictionaryKey);
        }
    }
}
