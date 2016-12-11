using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Accounts.Core.Helpers;
using Accounts.Core.Models;

namespace Accounts.Core.ModelBuilders
{
    public class PaymentModelBuilder : IPaymentModelBuilder
    {
        private readonly IPaymentHelper _paymentHelper;

        public PaymentModelBuilder(IPaymentHelper paymentHelper)
        {
            _paymentHelper = paymentHelper;
        }

        private DateTime GetDate(bool recurring, int payDay, DateTime date)
        {
            return recurring ? _paymentHelper.GetPaymentRecurringDate(payDay, date) : date;
        }

        public List<PaymentModel> Build(IDataReader reader)
        {
            var paymentModelFactoryList = new List<PaymentModel>();

            if (reader == null)
            {
                return paymentModelFactoryList;
            }

            while (reader.Read())
            {
                paymentModelFactoryList.Add(new PaymentModel
                {
                    Id = (Guid)reader["Id"],
                    Title = reader["Title"].ToString(),
                    Amount = decimal.Parse(reader["Amount"].ToString()),
                    PaidYearly = bool.Parse(reader["PaidYearly"].ToString()),
                    Recurring = bool.Parse(reader["Recurring"].ToString()),
                    Date = GetDate(bool.Parse(reader["Recurring"].ToString()), int.Parse(reader["PayDay"].ToString()), DateTime.Parse(reader["Date"].ToString())),
                    Paid = _paymentHelper.HasPaid(GetDate(bool.Parse(reader["Recurring"].ToString()), int.Parse(reader["PayDay"].ToString()), DateTime.Parse(reader["Date"].ToString())))
                });
            }

            return paymentModelFactoryList.OrderByDescending(x => x.Date).ToList();
        }
    }
}
