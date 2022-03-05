using Common.Models;
using FakeItEasy;
using FluentValidation.TestHelper;
using NUnit.Framework;
using PaymentGatewayAPI.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UKHO.FileShareService.API.UnitTests.Validation
{
    [TestFixture()]
    public class PaymentValidatorTests
    {
        private PaymentValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new PaymentValidator();
        }

        [Test]
        public void TestValidationForNullMerchantId()
        {
            var model = new PaymentRequest { MerchantId = null, payment = new Payment()};

            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(fb => fb.MerchantId);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "Merchant Id not present or not in guid format"));
        }

        //TODO: write test for other validation
    }
}
