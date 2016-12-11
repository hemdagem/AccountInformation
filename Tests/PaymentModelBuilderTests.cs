using System;
using System.Collections.Generic;
using Accounts.Core.Helpers;
using Accounts.Core.ModelBuilders;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PaymentModelBuilderTests
    {
        Mock<IPaymentHelper> _paymentHelperMock;
        private PaymentModelBuilder _builder;
        private void Setup()
        {
            _paymentHelperMock = new Mock<IPaymentHelper>();
            _builder =new PaymentModelBuilder(_paymentHelperMock.Object);
        }


        [Test]
        public void Build_should_return_empty_list_when_reader_is_null()
        {
            // given 
            Setup();

            // when
            var paymentModels = _builder.Build(null);

            // then
            Assert.AreEqual(0,paymentModels.Count);
        }

        [Test]
        public void Build_should_populate_core_list_model_when_reader_has_rows()
        {
            // given 
            Setup();
            var dictionary = new Dictionary<string, object>
            {
                {"Id", Guid.NewGuid()},
                {"Title", "Mobile"},
                {"Amount", 20.00},
                {"PaidYearly", true},
                {"PaymentTypeId", Guid.NewGuid()},
                {"Recurring", true},
                {"PayDay", 6},
                {"Date", DateTime.Today}
            };
            var dataReader = DataReaderTestHelper.Reader(dictionary);

            // when
            var paymentModels = _builder.Build(dataReader);

            // then
            Assert.AreEqual(1, paymentModels.Count);
        }
    }
}