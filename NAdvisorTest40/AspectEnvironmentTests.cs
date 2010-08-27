using System;
using System.Collections.Generic;
using NAdvisor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NAdvisorTest40
{
    [TestClass]
    public class AspectEnvironmentTests
    {
        [TestMethod]
        public void Should_have_Concrete_Type_in_AspectEnvironment()
        {
            Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> checkConcreteType = (environMent, aspects) =>
            {
                Assert.AreEqual(environMent.ConcretetType, typeof(Target));

                return aspects;
            };
            var proxiedClass = new Advisor(checkConcreteType, new List<IAspect>()).GetAdvicedProxy<ITarget>(new Target());

            proxiedClass.DoneSomething("test");
        }

        [TestMethod]
        public void Should_have_interface_type_in_AspectEnvironmen()
        {
            Func<IAspectEnvironment, IList<IAspect>, IList<IAspect>> checkConcreteType = (environMent, aspects) =>
            {
                Assert.AreEqual(environMent.InterfaceType, typeof(ITarget));

                return aspects;
            };
            var proxiedClass = new Advisor(checkConcreteType, new List<IAspect>()).GetAdvicedProxy<ITarget>(new Target());

            proxiedClass.DoneSomething("test");
        }
    }
}
