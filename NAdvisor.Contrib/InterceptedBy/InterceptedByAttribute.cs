using System;

namespace TechTalk.NAdvisor.Contrib
{
    public class InterceptedByAttribute : Attribute
    {
        private readonly Type[] _types;

        public InterceptedByAttribute(params Type[] types)
        {
            _types = types;
        }

        public Type[] Types { get { return _types; } }
    }
}