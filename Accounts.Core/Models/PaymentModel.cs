using System;

namespace Accounts.Core.Models
{
    public class PaymentModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool PaidYearly { get; set; }
        public Guid PaymentTypeId { get; set; }
        public bool Recurring { get; set; }
        public Guid IncomeId { get; set; }
        public string Title { get; set; }
        public bool Paid { get; set; }
        public int PayDay { get; set; }
    }
}