using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.NAdvisor.Core;

namespace NAdvisorTest
{
    [TestClass]
    public class AdvisorTests
    {
        [TestMethod]
        public void Should_Only_Allow_Interfaces_to_proxy1()
        {
            //Given
            var advisor = new Advisor(new List<IAspect>()); 
            
            //Then
            MSTestHelper.ShouldThrowException<ArgumentException>(() => advisor.GetAdvicedProxy<Target>(new Target()));
        }

        [TestMethod]
        public void Should_Only_Allow_Interfaces_to_proxy2()
        {
            //Given
            var advisor = new Advisor(new List<IAspect>());

            //Then
            MSTestHelper.ShouldThrowException<ArgumentException>(() => advisor.GetAdvicedProxy(typeof(Target), new Target()));
        }
        
    }
}
