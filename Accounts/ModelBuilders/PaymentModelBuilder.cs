using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public class PaymentModelBuilder : IPaymentModelBuilder
    {
        public Core.Models.PaymentModel BuildCoreModel(PaymentModel model)
        {
            return new Core.Models.PaymentModel
            {
                Id = model.Id,
                Amount = model.Amount,
                Date =model.Date,
                IncomeId = model.IncomeId,
                Paid = model.Paid,
                PaidYearly = model.PaidYearly,
                PayDay = model.PayDay,
                PaymentTypeId = model.PaymentTypeId,
                Recurring = model.Recurring,
                Title = model.Title,
            };
        }

        public PaymentModel BuildViewModel(Core.Models.PaymentModel model)
        {
            return new PaymentModel
            {
                Id = model.Id,
                Amount = model.Amount,
                Date = model.Date,
                IncomeId = model.IncomeId,
                Paid = model.Paid,
                PaidYearly = model.PaidYearly,
                PayDay = model.PayDay,
                PaymentTypeId = model.PaymentTypeId,
                Recurring = model.Recurring,
                Title = model.Title,
            };
        }
    }
}