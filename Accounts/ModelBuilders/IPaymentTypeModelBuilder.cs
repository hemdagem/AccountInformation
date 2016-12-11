using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public interface IPaymentTypeModelBuilder
    {
        Core.Models.PaymentTypes BuildCoreModel(PaymentTypes modelPaymentTypes);
        PaymentTypes BuildViewModel(Core.Models.PaymentTypes modelPaymentTypes);
    }
}