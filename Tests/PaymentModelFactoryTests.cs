using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Core.Helpers;
using Accounts.Core.Models;
using Accounts.ModelBuilders;
using Moq;
using NUnit.Framework;
using UserModel = Accounts.Models.UserModel;

namespace Tests
{
    [TestFixture]
    public class PaymentModelFactoryTests
    {
        [Test]
        public void should_return_empty_list_of_payments_when_reader_is_null()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            // when
            var payments = paymentModelFactory.CreatePayments(null);

            // then
            Assert.That(payments.Count, Is.EqualTo(0));
        }

        [Test]
        public void CreatePayments_should_return_list_of_payments_when_reader_has_rows()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            var Id = Guid.NewGuid();

            var dictionary = new Dictionary<string, object>
            {
                {"Id", Id},
                {"Title", "Mobile"},
                {"Amount", 20.00},
                {"PaidYearly", true},
                {"PaymentTypeId", Id},
                {"Recurring", true},
                {"PayDay", 6},
                {"Date", DateTime.Today}
            };
            // when
            var payments = paymentModelFactory.CreatePayments(new List<PaymentModel> { new PaymentModel { Id = Id, Title = "Mobile", Amount = 20.00M, PaidYearly = true, PaymentTypeId = Id, Recurring = true, PayDay = 6, Date = DateTime.Today } });

            // then
            Assert.That(payments.Count, Is.EqualTo(1));
            Assert.AreEqual(dictionary["Id"], payments[0].Id);
            Assert.AreEqual(dictionary["Title"], payments[0].Title);
            Assert.AreEqual(dictionary["Amount"], payments[0].Amount);
            Assert.AreEqual(dictionary["PaidYearly"], payments[0].PaidYearly);
            Assert.AreEqual(dictionary["PaymentTypeId"], payments[0].PaymentTypeId);
            Assert.AreEqual(dictionary["Recurring"], payments[0].Recurring);
            Assert.AreEqual(dictionary["PayDay"], 6);
        }

        [Test]
        public void CreatePaymentSummary_should_return_empty_object_when_payment_model_list_is_empty()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            // when
            var payments = paymentModelFactory.CreatePaymentSummary(new List<Accounts.Models.PaymentModel>(), new UserModel());

            // then
            Assert.AreEqual(0,payments.Amount);
            Assert.AreEqual(0,payments.CurrentAmountToPayThisMonth);
            Assert.AreEqual(0,payments.RemainingEachMonth);
            Assert.AreEqual(0,payments.TotalAmountToPayThisMonth);
            Assert.AreEqual(0,payments.YearlyAmountEachMonth);
            Assert.AreEqual(0,payments.Payments.Count());
        }

        [Test]
        public void CreatePaymentSummary_should_return_empty_object_when_payment_model_list_is_null()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            // when
            var payments = paymentModelFactory.CreatePaymentSummary(null, new UserModel());

            // then
            Assert.AreEqual(0, payments.Amount);
            Assert.AreEqual(0, payments.CurrentAmountToPayThisMonth);
            Assert.AreEqual(0, payments.RemainingEachMonth);
            Assert.AreEqual(0, payments.TotalAmountToPayThisMonth);
            Assert.AreEqual(0, payments.YearlyAmountEachMonth);
            Assert.AreEqual(0, payments.Payments.Count());
        }

    }
}