using System.Collections.Generic;
using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public interface IPaymentModelFactory
    {
        List<PaymentModel> CreatePayments(List<Accounts.Core.Models.PaymentModel> paymentModels);
        PaymentViewModel CreatePaymentSummary(List<PaymentModel> payments, UserModel model);
    }
}