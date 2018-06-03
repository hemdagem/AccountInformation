using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Core.Helpers;
using Accounts.Core.Models;
using Accounts.Web.ModelBuilders;
using Moq;
using Xunit;
using UserModel = Accounts.Web.Models.UserModel;

namespace Accounts.Tests.Unit.Accounts.Web.ModelBuilders
{
    
    public class PaymentModelFactoryTests
    {
        [Fact]
        public void should_return_empty_list_of_payments_when_reader_is_null()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            // when
            var payments = paymentModelFactory.CreatePayments(null);

            // then
            Assert.Equal(payments.Count,0);
        }

        [Fact]
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
                {"Recurring", true},
                {"PayDay", 6},
                {"Date", DateTime.Today}
            };
            // when
            var payments = paymentModelFactory.CreatePayments(new List<PaymentModel> { new PaymentModel { Id = Id, Title = "Mobile", Amount = 20.00M, PaidYearly = true, Recurring = true, PayDay = 6, Date = DateTime.Today } });

            // then
            Assert.Equal(payments.Count,1);
            Assert.Equal(dictionary["Id"], payments[0].Id);
            Assert.Equal(dictionary["Title"], payments[0].Title);
            Assert.NotStrictEqual(dictionary["Amount"], payments[0].Amount);
            Assert.Equal(dictionary["PaidYearly"], payments[0].PaidYearly);
            Assert.Equal(dictionary["Recurring"], payments[0].Recurring);
            Assert.Equal(dictionary["PayDay"], 6);
        }

        [Fact]
        public void CreatePaymentSummary_should_return_empty_object_when_payment_model_list_is_empty()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            // when
            var payments = paymentModelFactory.CreatePaymentSummary(new List<global::Accounts.Web.Models.PaymentModel>(), new UserModel());

            // then
            Assert.Equal(0,payments.Amount);
            Assert.Equal(0,payments.CurrentAmountToPayThisMonth);
            Assert.Equal(0,payments.RemainingEachMonth);
            Assert.Equal(0,payments.TotalAmountToPayThisMonth);
            Assert.Equal(0,payments.YearlyAmountEachMonth);
            Assert.Equal(0,payments.Payments.Count());
        }

        [Fact]
        public void CreatePaymentSummary_should_return_empty_object_when_payment_model_list_is_null()
        {
            // given
            Mock<IPaymentHelper> paymentHelperMock = new Mock<IPaymentHelper>();
            var paymentModelFactory = new PaymentModelFactory(paymentHelperMock.Object);

            // when
            var payments = paymentModelFactory.CreatePaymentSummary(null, new UserModel());

            // then
            Assert.Equal(0, payments.Amount);
            Assert.Equal(0, payments.CurrentAmountToPayThisMonth);
            Assert.Equal(0, payments.RemainingEachMonth);
            Assert.Equal(0, payments.TotalAmountToPayThisMonth);
            Assert.Equal(0, payments.YearlyAmountEachMonth);
            Assert.Equal(0, payments.Payments.Count());
        }

    }
}