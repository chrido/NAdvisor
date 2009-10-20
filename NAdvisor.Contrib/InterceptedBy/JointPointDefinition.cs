//   Copyright 2009 Christoph Doblander
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using NAdvisor.Core;

namespace NAdvisor.Contrib
{
    public class JointPointDefinition
    {
        public static IList<IAspect> AttributeBasedJointPointDefinition(Type type, IAspectEnvironment methodInfo, IList<IAspect> availableAspects)
        {
            object[] customAttributes = methodInfo.ConcreteMethodInfo.GetCustomAttributes(typeof(InterceptedByAttribute), true);
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