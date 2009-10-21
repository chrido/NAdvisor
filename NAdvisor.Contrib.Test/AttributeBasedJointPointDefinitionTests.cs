using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Moq;
using NAdvisor;
using NAdvisor.Core;

namespace NAdvisor.Contrib.Test
{
    [TestFixture]
    public class AttributeBasedJointPointDefinitionTests
    {
        [Test]
        public void Should_Intercept_Only_Attributed_Methods()
        {
            var aspectLog = new List<string>();
            var serviceLog = new List<string>();

            //Given
            var aspect = new SimpleAspect();
            aspect.DoneSomething += (obj, ea) => aspectLog.Add(ea.Message);
            var simpleService = new SimpleService();
            simpleService.DoneSomething += (obj, ea) => serviceLog.Add(ea.Message);
            var shouldNotBeCalledAspect = new Mock<IAspect>(MockBehavior.Strict);

            var advisor = new Advisor(JointPointDefinition.AttributeBasedJointPointDefinition, new List<IAspect>() {aspect, shouldNotBeCalledAspect.Object});
            var myService = advisor.GetAdvicedProxy<ISimpleService>(simpleService);

            //Then
            var result = myService.Dosomething("hallo");
            
            //When
            Assert.AreEqual(aspectLog[0], "aspectCalled");
            Assert.AreEqual(aspectLog[1], "hallo");
            Assert.AreEqual(aspectLog[2], "DoneSomething");

            Assert.AreEqual(result, "executed");

            Assert.AreEqual(serviceLog[0], "hallo");

            shouldNotBeCalledAspect.VerifyAll();
        }

        [Test]
        public void Should_Order_Aspects_From_Left_To_Right()
        {
            var aspectLog = new List<string>();
            var serviceLog = new List<string>();

            //Given
            var firstAspect = new FirstAspect();
            firstAspect.DoneSomething += (obj, ea) => aspectLog.Add(ea.Message);

            var secondAspect = new SecondAspect();
            secondAspect.DoneSomething += (obj, ea) => aspectLog.Add(ea.Message);
            
            var simpleService = new SimpleService();
            simpleService.DoneSomething += (obj, ea) => serviceLog.Add(ea.Message);
            var shouldNotBeCalledAspect = new Mock<IAspect>(MockBehavior.Strict);

            var advisor = new Advisor(JointPointDefinition.AttributeBasedJointPointDefinition, new List<IAspect>() { shouldNotBeCalledAspect.Object, secondAspect, shouldNotBeCalledAspect.Object, firstAspect });
            var myService = advisor.GetAdvicedProxy<ISimpleService>(simpleService);

            //Then
            var result = myService.DoMore("muchmore");

            shouldNotBeCalledAspect.VerifyAll();

            Assert.AreEqual(aspectLog.Count, 4);
            Assert.AreEqual("FirstAspectBefore", aspectLog[0]);
            Assert.AreEqual("SecondAspectBefore", aspectLog[1]);
            Assert.AreEqual("SecondAspectAfter", aspectLog[2]);
            Assert.AreEqual("FirstAspectAfter", aspectLog[3]);
            
        }
    }

    public class SimpleAspect : IAspect
    {
        public EventHandler<SimpleEventArgs> DoneSomething;

        public object Execute(Func<object[], object> proceedInvocation, object[] args, IAspectEnvironment method)
        {
            RaiseDoneSomething("aspectCalled");

            if (args.Length > 0)
            {
                if (args[0] is string)
                {
                    RaiseDoneSomething((string)args[0]);
                }
            }

            object result = proceedInvocation(args);

            if (result is string)
                RaiseDoneSomething((string)result);

            return "executed";
        }

        private void RaiseDoneSomething(string doneSomething)
        {
            if (DoneSomething != null)
                DoneSomething.Invoke(this, new SimpleEventArgs() { Message = doneSomething });
        }
    }

    public class FirstAspect : IAspect
    {
        public EventHandler<SimpleEventArgs> DoneSomething;

        public object Execute(Func<object[], object> proceedInvocation, object[] args, IAspectEnvironment method)
        {
            RaiseDoneSomething("FirstAspectBefore");

            object result = proceedInvocation(args);

            RaiseDoneSomething("FirstAspectAfter");
            return result;
        }

        private void RaiseDoneSomething(string doneSomething)
        {
            if (DoneSomething != null)
                DoneSomething.Invoke(this, new SimpleEventArgs() { Message = doneSomething });
        }
    }

    public class SecondAspect : IAspect
    {
        public EventHandler<SimpleEventArgs> DoneSomething;

        public object Execute(Func<object[], object> proceedInvocation, object[] args, IAspectEnvironment method)
        {
            RaiseDoneSomething("SecondAspectBefore");

            object result = proceedInvocation(args);

            RaiseDoneSomething("SecondAspectAfter");
            return result;
        }

        private void RaiseDoneSomething(string doneSomething)
        {
            if (DoneSomething != null)
                DoneSomething.Invoke(this, new SimpleEventArgs() { Message = doneSomething });
        }
    }

    public interface ISimpleService
    {
        string Dosomething(string arguments);

        string DoMore(string more);
    }

    public class SimpleService : ISimpleService
    {
        public EventHandler<SimpleEventArgs> DoneSomething;

        [InterceptedBy(typeof(SimpleAspect))]
        public string Dosomething(string arguments)
        {
            RaiseDoneSomething(arguments);
            return "DoneSomething";
        }

        [InterceptedBy(typeof(FirstAspect), typeof(SecondAspect))]
        public string DoMore(string more)
        {
            RaiseDoneSomething(more);
            return "more";
        }

        private void RaiseDoneSomething(string doneSomething)
        {
            if (DoneSomething != null)
                DoneSomething.Invoke(this, new SimpleEventArgs() { Message = doneSomething });
        }
    }
}