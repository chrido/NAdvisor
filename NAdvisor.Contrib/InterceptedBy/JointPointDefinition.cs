using System;
using System.Collections.Generic;
using TechTalk.NAdvisor.Core;

namespace TechTalk.NAdvisor.Contrib
{
    public class JointPointDefinition
    {
        public static IList<IAspect> AttributeBasedJointPointDefinition(IAspectEnvironment aspectEnvironment, IList<IAspect> availableAspects)
        {
            object[] customAttributes = aspectEnvironment.ConcreteMethodInfo.GetCustomAttributes(typeof(InterceptedByAttribute), true);
            if (customAttributes.Length == 0)
                return new List<IAspect>();                                 

            var takenAspects = new List<IAspect>();
            
            var attribute = customAttributes[0] as InterceptedByAttribute;
            if (attribute != null)
            {
                foreach (Type aspectTypes in attribute.Types) //Keeping order of Aspects
                {
                    foreach (IAspect aspect in availableAspects)
                    {
                        if(aspect.GetType() == aspectTypes) //return the Aspect if the Types Match
                        {
                            takenAspects.Add(aspect);
                            break;
                        }
                    }
                }
            }

            return takenAspects;
        }
    }
}