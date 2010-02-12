using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAdvisor.Core;

namespace NAdvisor.Contrib.Test
{
    [TestClass]
    public class SampleMemozizationTest
    {
        [TestMethod]
        public void Should_Memoize_Method_Result_On_Second_Call()
        {
            //Given
            var aspect = new TestAspect();
            var received = new List<string>();
            aspect.Executed += (sender, args) => received.Add(args.Message);

            var service = new Memoservice();
            var advicedProxy = new Advisor(
                new MemoizeJointPointDefinition(VeryLongRunningAspectCalculation).GetMemoizedJointPointDefinition()
                , new List<IAspect>() { aspect }).GetAdvicedProxy<IMemoService>(service);

            //Then
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            ResultObject retrieval = advicedProxy.DoAwfulLongRetrieval("test");
            stopwatch.Stop();

            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 800);
            //When

            stopwatch.Reset();
            stopwatch.Start();

            advicedProxy.DoAwfulLongRetrieval("bamm");

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 100);

        }

        private IList<IAspect> VeryLongRunningAspectCalculation(IAspectEnvironment arg2, IList<IAspect> arg3)
        {
            Thread.Sleep(1000);
            return arg3;
        }
    }

    public class ResultObject
    {
        public string Result { get; set; }
    }

    public class TestEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class TestAspect : IAspect
    {
        public EventHandler<TestEventArgs> Executed;

        public object Execute(Func<object[], object> proceedInvocation, object[] methodArguments, IAspectEnvironment method)
        {
            Executed.Invoke(this, new TestEventArgs() { Message = "before" });
            return proceedInvocation(methodArguments);
        }
    }

    public interface IMemoService
    {
        ResultObject DoAwfulLongRetrieval(string parameter1);

        void UpdateParameter(ResultObject resultObject);
    }

    public class Memoservice : IMemoService
    {
        public ResultObject DoAwfulLongRetrieval(string parameter1)
        {
            return new ResultObject() { Result = "result" };
        }

        public void UpdateParameter(ResultObject resultObject)
        {
            //updateLogic of external System
        }
    }
}
