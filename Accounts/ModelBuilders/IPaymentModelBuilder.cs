using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public interface IPaymentModelBuilder
    {
        Core.Models.PaymentModel BuildCoreModel(PaymentModel model);
        PaymentModel BuildViewModel(Core.Models.PaymentModel model);
    }
}