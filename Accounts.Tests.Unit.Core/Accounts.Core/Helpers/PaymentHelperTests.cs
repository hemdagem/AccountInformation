using System;
using Accounts.Core.Helpers;
using Moq;

using Xunit;

namespace Accounts.Tests.Unit.Accounts.Core.Helpers
{
    public class PaymentHelperTests
    {
        Mock<IClock> _clock;
        private PaymentHelper _paymentHelper;

        public PaymentHelperTests()
        {
            _clock = new Mock<IClock>();
            _clock.Setup(x => x.GetDateTime()).Returns(new DateTime(2015, 10, 10));
        }

        [Fact]
        public void should_throw_exception_if_pay_day_is_less_than_1_for_getting_payday()
        {
            // given
            _paymentHelper = new PaymentHelper(_clock.Object);

            //when + then

            Assert.Throws<ArgumentOutOfRangeException>(() =>_paymentHelper.GetPreviousPayDay(0));
        }

        [Fact]
        public void should_throw_exception_if_pay_day_is_greater_than_31_for_getting_payday()
        {
            // given
            _paymentHelper = new PaymentHelper(_clock.Object);

            //when + then
            Assert.Throws<ArgumentOutOfRangeException>(() => _paymentHelper.GetPreviousPayDay(32));
        }

        [Fact]
        public void should_throw_exception_if_pay_day_is_less_than_1_for_getting_next_payday()
        {
            //when + then
            _paymentHelper = new PaymentHelper(_clock.Object);
           
            Assert.Throws<ArgumentOutOfRangeException>(() => _paymentHelper.GetNextPayDay(0));

        }

        [Fact]
        public void should_throw_exception_if_pay_day_is_greater_than_31_for_getting_next_payday()
        {
            // given
            _paymentHelper = new PaymentHelper(_clock.Object);

            //when + then
            Assert.Throws<ArgumentOutOfRangeException>(() => _paymentHelper.GetNextPayDay(32));
        }

        [Fact]
        public void should_set_month_to_Janaury_if_month_is_December()
        {
            // given
            _clock.Setup(x => x.GetDateTime()).Returns(new DateTime(2015, 12, 10));
            _paymentHelper = new PaymentHelper(_clock.Object);

            // when
            var nextPayDay = _paymentHelper.GetNextPayDay(10);

            // then
            Assert.Equal(2016, nextPayDay.Year);
            Assert.Equal(1, nextPayDay.Month);
        }

        [Fact]
        public void should_show_previous_month_when_payday_has_not_been_reached_but_the_month_has_changed()
        {
            // given
            _clock.Setup(x => x.GetDateTime()).Returns(new DateTime(2015, 10, 5));
            _paymentHelper = new PaymentHelper(_clock.Object);

            // when
            var paymentRecurringDate = _paymentHelper.GetPaymentRecurringDate(6, new DateTime(2015, 9, 20));

            // then
            Assert.Equal(9, paymentRecurringDate.Month);
        }

        [Fact]
        public void should_show_current_month_when_payday_has_been_reached_and_the_month_has_changed()
        {
            // given
            _paymentHelper = new PaymentHelper(_clock.Object);

            // when
            var paymentRecurringDate = _paymentHelper.GetPaymentRecurringDate(10, new DateTime(2015, 10, 10));

            // then
            Assert.Equal(10, paymentRecurringDate.Month);
        }
    }
}