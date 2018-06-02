using Accounts.Web.Models;
using Xunit;


namespace Accounts.Tests.Unit.Accounts.Web.Models
{
    
    public class PaymentViewModelTests
    {
        [Fact]
        public void PaymentsListShouldNotBeNull()
        {
            var paymentViewModel = new PaymentViewModel();

            Assert.NotNull(paymentViewModel.Payments);
        }
    }
}