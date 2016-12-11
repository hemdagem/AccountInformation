using System.Collections.Generic;

namespace Accounts.Web.Models
{
    public class PaymentViewModel
    {
        public IEnumerable<PaymentModel> Payments { get; set; } 
        public decimal Amount { get; set; } 
        public decimal RemainingEachMonth { get; set; } 
        public decimal YearlyAmountEachMonth { get; set; } 
        public decimal CurrentAmountToPayThisMonth { get; set; }
        public decimal TotalAmountToPayThisMonth { get; set; }

        public PaymentViewModel()
        {
            Payments = new List<PaymentModel>();
        }
    }
}