using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeAdvice.Aspects;
using FreeAdvice.Common;
using FreeAdvice.Domain;
using FreeAdvice.Repositories.Interfaces;
using FreeAdvice.Service.Interfaces;
using FreeAdvice.Services;
using Moq;
using NAdvisor.Contrib;
using NAdvisor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FreeAdvice.MSTest
{
    [TestClass]
    public class AdviceServiceTest
    {
        [TestMethod]
        public void Should_Validate_Advice_Dto()
        {
            //Given
            var advice = new AdviceDto();
            var service = new AdviceService(null, null, null);

            //Then
            MSTestHelper.ShouldThrowException<ValidationException>(() => service.UpdateAdvice(advice));
        }


        /// <summary>
        /// Note: May not be the typical goog unit Test but shows how much you can abuse it
        /// </summary>
        [TestMethod]
        public void Should_Update_Repository_And_Commit_To_External_Dms_When_Validated()
        {
            //Constants
            var documentFolder = Guid.NewGuid();
            
            //Given
            var advice = new AdviceDto() {AdviceText = "adsfasdfasdf", Id = default(Guid), RandomNumber = 3};
            var mockFactory = new MockFactory(MockBehavior.Strict);

            
            //Service Dependencies
            var adviceRepository = mockFactory.Create<IAdviceRepository>();
            var configuration = mockFactory.Create<IConfiguration>();

            var aspects = new List<IAspect>();
            var transactionContext = mockFactory.Create<ITransactionContext>();
            var dms = mockFactory.Create<IDms>();
            //Aspect Configuration

            aspects.Add(new DatabaseContext(transactionContext.Object));
            aspects.Add(new ExternalDmsContext(dms.Object));

            var advisor = new Advisor(JointPointDefinition.AttributeBasedJointPointDefinition, aspects);
            var service = advisor.GetAdvicedProxy<IAdviceService>(new AdviceService(adviceRepository.Object, dms.Object, configuration.Object));
            
            //setup Mocks
            transactionContext.Setup(tc => tc.Push()).AtMost(1);
            configuration.Setup(co => co.GetDefaultDocumentFolder).Returns(documentFolder).AtMostOnce();
            dms.Setup(dm => dm.AddDocumentToFolder(It.IsAny<Guid>(), It.IsAny<byte[]>(), It.IsAny<string>())).AtMostOnce();
            adviceRepository.Setup(ar => ar.UpdateAdvice(advice)).AtMostOnce();
            dms.Setup(dm => dm.Commit()).AtMostOnce();
            transactionContext.Setup(tc => tc.Commit());
            
            //Then
            service.UpdateAdvice(advice);

            //When

            mockFactory.VerifyAll();
        }
    }
}
