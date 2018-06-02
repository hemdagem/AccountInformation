namespace Accounts.Core.Models
{
    public class PaymentViewModel
    {
        public decimal CurrentAmountToPayThisMonth { get; set; }
        public decimal YearlyAmountEachMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingEachMonth { get; set; }
        public decimal TotalAmountToPayThisMonth { get; set; }
    }
}