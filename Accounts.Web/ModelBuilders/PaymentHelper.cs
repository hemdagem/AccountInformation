using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Core.Helpers;
using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public class PaymentHelper : IPaymentHelper
    {
        private readonly IClock _clock;

        public PaymentHelper(IClock clock)
        {
            _clock = clock;
        }

        public DateTime GetPaymentRecurringDate(int payDay, DateTime paymentDate)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            var date = _clock.GetDateTime();

            if (payDay > date.Day)
            {
                if (paymentDate.Day >= payDay)
                {
                    return new DateTime(date.Year, date.Month - 1, paymentDate.Day);
                }

                return new DateTime(date.Year, date.Month, paymentDate.Day);
            }

            var month = date.Month;

            if (paymentDate.Day < date.Day)
            {
                month = month - 1;
            }

            return new DateTime(date.Year, month, paymentDate.Day);
        }

        private DateTime GetPreviousPayDay(int payDay)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            var date = _clock.GetDateTime();

            return new DateTime(date.Year, date.Month, payDay);
        }

        private DateTime GetNextPayDay(int payDay)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            var date = _clock.GetDateTime();

            return new DateTime(date.Year, date.Month + 1, payDay);
        }

        public bool HasPaid(int payDay, DateTime paymentDate)
        {
            var date = _clock.GetDateTime();

            return paymentDate <= date;
        }

        public decimal GetRemainingEachMonth(decimal amount, List<PaymentModel> paymentModelList)
        {
            return amount - paymentModelList.Sum(x => x.Amount);
        }

        public decimal GetCurrentAmountToPayThisMonth(int payDay, List<PaymentModel> paymentModelList)
        {
            return paymentModelList.Where(x => x.Date <= GetNextPayDay(payDay) && x.Date > DateTime.UtcNow).Sum(x => x.Amount);
        }

        public decimal GetTotalAmountToPayThisMonthh(int payDay,List<PaymentModel> paymentModelList)
        {
            return paymentModelList.Where(x => (x.Date >= GetPreviousPayDay(payDay) && x.Date <= GetNextPayDay(payDay)) || x.Recurring).Sum(x => x.Amount);
        }
    }
}