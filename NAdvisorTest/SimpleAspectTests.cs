using System;
using System.Collections.Generic;
using MbUnit.Framework;
using NAdvisor.Core;

namespace NAdvisorTest.SimpleAspectTests
{
    [TestFixture]
    public class SimpleAspectTests
    {
        [Test]
        public void Should_Intercept_By_Aspects()
        {
            //Given
            var results = new List<string>();
            var simpleAspect = new SimpleAspect();
            var simpleService = new SimpleService();
            simpleAspect.DoneSomething += (sender, args) => results.Add(args.Message);

            var advisor = new Advisor(new List<IAspect>() {simpleAspect});
            var service = advisor.GetAdvicedProxy<ISimpleService>(simpleService);

            //Then
            var result = service.DoSomething("hllo");

            //When
            Assert.AreEqual("executed", result);
            Assert.AreEqual("hllo", results[0]);
            Assert.AreEqual("donesomething", results[1]);
        }
    }

    public class SimpleAspect : IAspect
    {
        public EventHandler<SimpleEventArgs> DoneSomething;

        public object Execute(Func<object[], object> proceedInvocation, object[] args, IAspectEnvironment method)
        {
            if(args.Length > 0)
            {
                if(args[0] is string)
                {
                    RaiseDoneSomething((string)args[0]);
                }
            }

            object result = proceedInvocation(args);

            if (result is string)
                RaiseDoneSomething((string) result);
            
            return "executed";
        }

        private void RaiseDoneSomething(string doneSomething)
        {
            if (DoneSomething != null)
                DoneSomething.Invoke(this, new SimpleEventArgs() { Message = doneSomething });
        }
    }

    public interface ISimpleService
    {
        string DoSomething(string somethingTodo);
    }

    public class SimpleService : ISimpleService
    {
        public EventHandler<SimpleEventArgs> DoneSomething;

        public string DoSomething(string somethingTodo)
        {
            if(DoneSomething != null)
                DoneSomething.Invoke(this, new SimpleEventArgs(){Message = somethingTodo});

            return "donesomething";
        }
    }
}
