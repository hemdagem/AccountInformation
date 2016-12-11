using System;

namespace Accounts.Core.Helpers
{
    public interface IPaymentHelper
    {
        DateTime GetPaymentRecurringDate(int payDay, DateTime paymentDate);
        DateTime GetPreviousPayDay(int payDay);
        DateTime GetNextPayDay(int payDay);
        bool HasPaid(DateTime paymentDate);
    }
}