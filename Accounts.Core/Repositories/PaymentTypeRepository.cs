using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Accounts.Core.Models;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Database.DataAccess.Interfaces;

namespace Accounts.Core.Repositories
{
    public class PaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly IDbConnectionFactory _dataAccess;

        public PaymentTypeRepository(IDbConnectionFactory dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<ListItem>> GetPaymentTypes()
        {
            var reader = await _dataAccess.ExecuteReader("up_GetPaymentTypes");
            var items = new List<ListItem>();

            while (reader.Read())
            {
                items.Add(new ListItem
                {
                    Text = reader["Title"].ToString(),
                    Value = reader["Id"].ToString()
                });
            }

            return items;
        }

        public async Task<Guid> AddPaymentType(PaymentTypes model)
        {
            return _dataAccess.ExecuteScalar<Guid>("up_AddPaymentType", new SqlParameter("Title", model.Title));
        }

        public async Task<int> UpdatePaymentType(PaymentTypes model)
        {
            return _dataAccess.ExecuteScalar<int>("up_UpdatePaymentType", new SqlParameter("Id", model.Id), new SqlParameter("Title", model.Title));
        }

        public async Task<PaymentTypes> GetPaymentType(Guid id)
        {
            var reader = await _dataAccess.ExecuteReader("up_GetPaymentType", new SqlParameter("Id", id));
            var paymentTypes = new PaymentTypes();
            while (reader.Read())
            {
                paymentTypes.Id = (Guid) reader["Id"];
                paymentTypes.Title = reader["Title"].ToString();
            }

            return paymentTypes;
        }
    }
}