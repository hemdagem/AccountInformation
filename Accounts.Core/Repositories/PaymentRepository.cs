using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Accounts.Core.ModelBuilders;
using Accounts.Core.Models;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Database.DataAccess.Interfaces;

namespace Accounts.Core.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDbConnectionFactory _dataAccess;
        private readonly IPaymentModelBuilder _paymentModelBuilder;

        public PaymentRepository(IDbConnectionFactory dataAccess, IPaymentModelBuilder paymentModelBuilder)
        {
            _dataAccess = dataAccess;
            _paymentModelBuilder = paymentModelBuilder;
        }

        public async Task<List<PaymentModel>> GetPaymentsById(Guid userId)
        {
            var userIdParameter = new SqlParameter("UserId", userId);

            var paymentList = await _dataAccess.ExecuteReader("up_GetPaymentsById", userIdParameter);

            if (paymentList == null)
            {
                throw new NullReferenceException();
            }

            var paymentModels = _paymentModelBuilder.Build(paymentList);

            return paymentModels;

        }

        public async Task<Guid> AddPayment(PaymentModel model)
        {
            var idParameter = new SqlParameter("IncomeId", model.IncomeId);
            var amountParameter = new SqlParameter("Amount", model.Amount);
            var dateParameter = new SqlParameter("Date", model.Date);
            var paidYearlyParameter = new SqlParameter("PaidYearly", model.PaidYearly);
            var recurringParameter = new SqlParameter("Recurring", model.Recurring);
            var paymentTypeIdParameter = new SqlParameter("PaymentTypeId", model.PaymentTypeId);

            var paymentList = _dataAccess.ExecuteScalar<Guid>("up_AddPayment", idParameter, amountParameter, dateParameter, paidYearlyParameter, paymentTypeIdParameter, recurringParameter);

            if (paymentList == Guid.Empty)
            {
                throw new NullReferenceException();
            }

            return paymentList;
        }

        public async Task<int> UpdatePayment(PaymentModel model)
        {
            var idParameter = new SqlParameter("Id", model.Id);
            var amountParameter = new SqlParameter("Amount", model.Amount);
            var dateParameter = new SqlParameter("Date", model.Date);
            var paidYearlyParameter = new SqlParameter("PaidYearly", model.PaidYearly);
            var paymentTypeIdParameter = new SqlParameter("PaymentTypeId", model.PaymentTypeId);
            var recurringParameter = new SqlParameter("Recurring", model.Recurring);

            return _dataAccess.ExecuteScalar<int>("up_UpdatePayment", amountParameter, dateParameter, paidYearlyParameter, paymentTypeIdParameter, idParameter, recurringParameter);
        }

        public async Task<int> DeletePayment(Guid guid)
        {
            return _dataAccess.ExecuteScalar<int>("up_DeletePayment", new SqlParameter("PaymentId", guid));
        }

        public async Task<List<ListItem>> GetPaymentTypes()
        {
            var paymentTypes =  await _dataAccess.ExecuteReader("up_GetPaymentTypes");
            var items = new List<ListItem>();

            while (paymentTypes.Read())
            {
                items.Add(new ListItem
                {
                    Text = paymentTypes["Title"].ToString(),
                    Value = paymentTypes["Id"].ToString()
                });
            }

            return items;
        }


    }
}