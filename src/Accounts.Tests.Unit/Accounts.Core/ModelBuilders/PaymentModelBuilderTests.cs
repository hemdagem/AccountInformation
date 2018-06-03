using System;
using System.Collections.Generic;
using Accounts.Core.Helpers;
using Accounts.Core.ModelBuilders;
using Accounts.Tests.Unit.TestHelpers;
using Moq;
using Xunit;


namespace Accounts.Tests.Unit.Accounts.Core.ModelBuilders
{
    
    public class PaymentModelBuilderTests
    {
        Mock<IPaymentHelper> _paymentHelperMock;
        private PaymentModelBuilder _builder;

        public PaymentModelBuilderTests()
        {
            _paymentHelperMock = new Mock<IPaymentHelper>();
            _builder =new PaymentModelBuilder(_paymentHelperMock.Object);
        }


        [Fact]
        public void Build_should_return_empty_list_when_reader_is_null()
        {
            //given + when
            var paymentModels = _builder.Build(null);

            // then
            Assert.Empty(paymentModels);
        }

        [Fact]
        public void Build_should_populate_core_list_model_when_reader_has_rows()
        {
            // given
            var dictionary = new Dictionary<string, object>
            {
                {"Id", Guid.NewGuid()},
                {"Title", "Mobile"},
                {"Amount", 20.00},
                {"PaidYearly", true},
                {"Recurring", true},
                {"PayDay", 6},
                {"Date", DateTime.Today}
            };
            var dataReader = DataReaderTestHelper.Reader(dictionary);

            // when
            var paymentModels = _builder.Build(dataReader);

            // then
            Assert.Single(paymentModels);
        }
    }
}