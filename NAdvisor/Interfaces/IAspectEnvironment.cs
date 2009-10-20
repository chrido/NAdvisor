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
        /// Method Info of the Interface Method
        /// </summary>
        MethodInfo InterfaceMethodInfo { get;}

        /// <summary>
        /// Concrete Object which implements the interface which is proxied
        /// </summary>
        object ConcreteObject { get; }

        T TryGetValue<T>(string key);

        void SetValue(string key, object value);
    }
}