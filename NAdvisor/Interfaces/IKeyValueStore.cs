namespace NAdvisor.Core
{
    public interface IKeyValueStore
    {
        /// <summary>
        /// Sets a value, replaces when already there
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetValue(string key, object value);

        /// <summary>
        /// Does not throw any exception
        /// if not found the default value is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T TryGetValue<T>(string key);

        /// <summary>
        /// Behaviour
        /// 
        /// Throws exception if key is not found and does not implements the fiven type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// Checks if the key is in keyValuestore
        /// </summary>
        /// <param name="dictionaryKey"></param>
        /// <returns></returns>
        bool ContainsKey(string dictionaryKey);
    }
}