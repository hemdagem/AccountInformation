using Accounts.Models;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PaymentViewModelTests
    {
        [Test]
        public void PaymentsListShouldNotBeNull()
        {
            var paymentViewModel = new PaymentViewModel();

            Assert.IsNotNull(paymentViewModel.Payments);
        }
    }
}