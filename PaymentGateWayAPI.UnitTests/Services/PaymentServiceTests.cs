using Common.Models;
using FakeItEasy;
using FluentValidation.TestHelper;
using NUnit.Framework;
using PaymentGatewayAPI.Repositories;
using PaymentGatewayAPI.Services;
using PaymentGatewayAPI.Helpers;
using PaymentGatewayAPI.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Enums;

namespace UKHO.FileShareService.API.UnitTests.Validation
{
    [TestFixture()]
    public class PaymentServiceTests
    {
        private PaymentService Service;

        private IPaymentRepository fakeRepository;

        private IBankSimulatorClient fakebankSimulatorClient;
        [SetUp]
        public void Setup()
        {
            fakeRepository = A.Fake<IPaymentRepository>();
            fakebankSimulatorClient = A.Fake<IBankSimulatorClient>();

            Service = new PaymentService(fakeRepository, fakebankSimulatorClient);
        }

        [Test]
        public void TestValidationForCardMasking()
        {
            A.CallTo(() => fakeRepository.GetPaymentsById(A<Guid>.Ignored))
                .Returns(new List<MerchantPaymentDetails>
                {
                    new MerchantPaymentDetails()
                    {
                        MerchantPaymentId = Guid.NewGuid(),
                        Status = PaymentStatus.Approved,
                        Payment = new Payment()
                        {
                            Currency = Currency.US,
                            Amount = 123,
                            CardNumber = "1111-2222-3333-4444",
                            CVV = 123,
                            ExpiryMonthYear = "122022",
                            NameOnCard  = "ABC"
                        }
                    }
                });

            var result = Service.GetPaymentListByMerchant(Guid.NewGuid());

            Assert.AreEqual(result.ToList()[0].Payment.CardNumber, "XXXX-XXXX-XXXX-4444");
        }

        [Test]
        public async Task TestValidationForPayment()
        {
            A.CallTo(() => fakebankSimulatorClient.TakePayment(A<Payment>.Ignored))
                .Returns(Task.FromResult(new AccuringBankResponse { Message = "Paid", Status= PaymentStatus.Approved}));

            A.CallTo(() => fakeRepository.AddPayment(A<MerchantPaymentDetails>.Ignored)).DoesNothing();

            var result = await Service.DoPaymentAsync(new PaymentRequest { MerchantId = Guid.NewGuid(), payment = new Payment()});
        
            A.CallTo(() => fakeRepository.AddPayment(A<MerchantPaymentDetails>.Ignored)).MustHaveHappened();
            Assert.AreEqual(PaymentStatus.Approved, result.Status);
            Assert.AreEqual("Paid",result.Message);
        }
        //TODO: write test for other business logic
    }

}