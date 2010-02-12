using System;
using System.Collections.Generic;
using System.Reflection;

namespace NAdvisor.Core
{
    public interface IAspectEnvironment
    {
        /// <summary>
        /// Method Info of the Concrete Implementation
        /// </summary>
        MethodInfo ConcreteMethodInfo { get; }

        /// <summary>
        /// Type of the Target
        /// </summary>
        Type ConcretetType { get; }

        /// <summary>
        /// Type of the interface
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// Method Info of the Interface Method
        /// </summary>
        MethodInfo InterfaceMethodInfo { get;}

        /// <summary>
        /// Concrete Object which implements the interface which is proxied
        /// </summary>
        object ConcreteObject { get; }

        /// <summary>
        /// Tries to get a value, 
        /// !! care about locking, possible race condition, deadlocks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T TryGetValue<T>(string key);

        /// <summary>
        /// Sets a Value in the EnvironmentDictionary
        /// !! care about locking, possible race condition, deadlocks
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetValue(string key, object value);

        /// <summary>
        /// Syncronices within the Aspectproxy
        /// => !! Raceconditions and deadlocks can occure depending on the lifetime, or sharing between other threads
        /// </summary>
        /// <param name="action"></param>
        void SyncronizedKeyValueStoreMutator(Action<IKeyValueStore> action);

        /// <summary>
        /// Syncroniced Access to the key value store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getOrCreateFunc"></param>
        object GetValueOrCreate(string key, Func<object> getOrCreateFunc);
    }
}