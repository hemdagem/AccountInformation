using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Core.Helpers;
using Accounts.Web.Models;

namespace Accounts.Web.ModelBuilders
{
    public class PaymentModelFactory : IPaymentModelFactory
    {
        private readonly IPaymentHelper _paymentHelper;

        public PaymentModelFactory(IPaymentHelper paymentHelper)
        {
            _paymentHelper = paymentHelper;
        }

        public List<PaymentModel> CreatePayments(List<Accounts.Core.Models.PaymentModel> paymentModels)
        {
            var paymentModelFactoryList = new List<PaymentModel>();

            if (paymentModels == null)
            {
                return paymentModelFactoryList;
            }

            foreach (var paymentModel in paymentModels)
            {
                paymentModelFactoryList.Add(new PaymentModel
                {
                    Id = paymentModel.Id,
                    Title = paymentModel.Title,
                    Amount = paymentModel.Amount,
                    PaidYearly = paymentModel.PaidYearly,
                    Recurring = paymentModel.Recurring,
                    Date =paymentModel.Date,
                    Paid = paymentModel.Paid
                });
            }

            return paymentModelFactoryList.OrderByDescending(x=>x.Date).ToList();
        }

        public PaymentViewModel CreatePaymentSummary(List<PaymentModel> payments, UserModel model)
        {
            var paymentModel = new PaymentViewModel();

            if (payments == null)
            {
                return paymentModel;
            }

            paymentModel.Amount = model.Amount;
            paymentModel.Payments = payments;
            paymentModel.RemainingEachMonth = model.Amount - payments.Sum(x => x.Amount);
            paymentModel.YearlyAmountEachMonth = payments.Where(x => x.PaidYearly).Sum(x => x.Amount);

            var nextPayDay = _paymentHelper.GetNextPayDay(model.PayDay);
            var previousPayDay = _paymentHelper.GetPreviousPayDay(model.PayDay);

            paymentModel.CurrentAmountToPayThisMonth = payments.Where(x => x.Date <= nextPayDay && x.Date > DateTime.UtcNow).Sum(x => x.Amount);
            paymentModel.TotalAmountToPayThisMonth = payments.Where(x => (x.Date >= previousPayDay && x.Date <= nextPayDay) || x.Recurring).Sum(x => x.Amount);

            return paymentModel;
        }
    }
}