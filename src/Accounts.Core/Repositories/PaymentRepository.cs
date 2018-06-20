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

        public async Task<Guid> AddPayment(PaymentModel paymentModel)
        {
            var idParameter = new SqlParameter("IncomeId", paymentModel.IncomeId);
            var amountParameter = new SqlParameter("Amount", paymentModel.Amount);
            var dateParameter = new SqlParameter("Date", paymentModel.Date);
            var paidYearlyParameter = new SqlParameter("PaidYearly", paymentModel.PaidYearly);
            var recurringParameter = new SqlParameter("Recurring", paymentModel.Recurring);
            var titleParameter = new SqlParameter("Title", paymentModel.Title);

            var paymentList = await _dataAccess.ExecuteScalar<Guid>("up_AddPayment", idParameter, amountParameter, dateParameter, paidYearlyParameter, recurringParameter, titleParameter);

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
            var recurringParameter = new SqlParameter("Recurring", model.Recurring);
            var titleParameter = new SqlParameter("Title", model.Title);

            return await _dataAccess.ExecuteScalar<int>("up_UpdatePayment", amountParameter, dateParameter, paidYearlyParameter, idParameter, recurringParameter, titleParameter);
        }

        public async Task<int> DeletePayment(Guid guid)
        {
            return await _dataAccess.ExecuteScalar<int>("up_DeletePayment", new SqlParameter("PaymentId", guid));
        }
    }
}