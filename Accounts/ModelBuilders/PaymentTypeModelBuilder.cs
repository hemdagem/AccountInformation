using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public class PaymentTypeModelBuilder : IPaymentTypeModelBuilder
    {
        public Core.Models.PaymentTypes BuildCoreModel(PaymentTypes modelPaymentTypes)
        {
            return new Core.Models.PaymentTypes
            {
                Id = modelPaymentTypes.Id,
                Title = modelPaymentTypes.Title
            };

        }

        public PaymentTypes BuildViewModel(Core.Models.PaymentTypes modelPaymentTypes)
        {
            return new PaymentTypes
            {
                Id = modelPaymentTypes.Id,
                Title = modelPaymentTypes.Title
            };

        }
    }
}