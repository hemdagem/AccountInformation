using Accounts.Web.Models;
using NUnit.Framework;

namespace Accounts.Tests.Unit.Accounts.Web.Models
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