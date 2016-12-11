using System;
using System.Collections.Generic;
using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public interface IPaymentHelper
    {
        DateTime GetPaymentRecurringDate(int payDay, DateTime paymentDate);
        bool HasPaid(int payDay, DateTime paymentDate);
        decimal GetRemainingEachMonth(decimal amount, List<PaymentModel> paymentModelList);
        decimal GetCurrentAmountToPayThisMonth(int payDay, List<PaymentModel> paymentModelList);
        decimal GetTotalAmountToPayThisMonthh(int payDay, List<PaymentModel> paymentModelList);

    }
}