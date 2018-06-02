using System.Collections.Generic;
using Accounts.Web.Models;

namespace Accounts.Web.ModelBuilders
{
    public interface IPaymentModelFactory
    {
        List<PaymentModel> CreatePayments(List<Accounts.Core.Models.PaymentModel> paymentModels);
        PaymentViewModel CreatePaymentSummary(List<PaymentModel> payments, UserModel model);
    }
}