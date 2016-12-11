using System;

namespace Accounts.Web.Models
{
    public class PaymentModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public bool PaidYearly { get; set; }
        public bool Paid { get; set; }
        public DateTime Date { get; set; }
        public Guid IncomeId { get; set; }
        public bool Recurring { get; set; }
        public string Title { get; set; }
        public int PayDay { get; set; }
    }
}