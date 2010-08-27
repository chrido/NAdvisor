using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAdvisor.Core;
using Moq;

namespace NAdvisorTest40
{
    [TestClass]
    public class AspectEnvironmentDictionaryTests
    {
        [TestMethod]
        public void Should_be_able_to_get_and_set_values_in_the_AspectEnvironment()
        {
            //Given
            var target = new Target();

            Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> jointPointDefinition = JointPointDefinition;
            var proxiedTarget = new Advisor(jointPointDefinition, new List<IAspect>()).GetAdvicedProxy<ITarget>(target);


            //Then
            proxiedTarget.DoneSomething("test");

            //When

        }

        private IList<IAspect> JointPointDefinition(IAspectEnvironment environMent, IList<IAspect> aspectList)
        {

            return aspectList;
        }

        [TestMethod]
        public void Should_put_value_in_KeyValueStore()
        {
            //Given
            var keyValueStore = new Mock<IKeyValueStore>();
            keyValueStore
                .Setup(kv => kv.SetValue(It.Is<string>(st => st == "test"), It.IsAny<object>()))
                .AtMostOnce();

            Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> kvSetValueFunc = 
                (environment, aspect) => 
                { 
                    environment.SetValue("test", new object());
                    return aspect; 
                };
            var proxiedTarget = new Advisor(kvSetValueFunc, new List<IAspect>()).GetAdvicedProxy<ITarget>(new Target(), keyValueStore.Object);

            //Then

            proxiedTarget.DoneSomething("bamm");

            //When
            keyValueStore.VerifyAll();
        }
    }
}
