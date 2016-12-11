using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Accounts.Core.Models;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Database.DataAccess.Interfaces;

namespace Accounts.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dataAccess;

        public UserRepository(IDbConnectionFactory dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<ListItem>> GetUsers()
        {
            var reader =  await _dataAccess.ExecuteReader("up_GetUsers");
            var items = new List<ListItem>();

            while (reader.Read())
            {
                items.Add(new ListItem
                {
                    Text = reader["Name"].ToString(),
                    Value = reader["Id"].ToString()
                });
            }
            return items;
        }

        public async Task<Guid> AddUser(UserModel model)
        {
            var nameParameter = new SqlParameter("Name", model.Name);
            var amountParameter = new SqlParameter("Amount", model.Amount);
            var payDayParameter = new SqlParameter("PayDay", model.PayDay);

            var task = _dataAccess.ExecuteScalar<Guid>("up_AddUser", nameParameter, amountParameter, payDayParameter);
            return task;
        }

        public async Task<int> UpdateUser(UserModel model)
        {
            var userIdParameter = new SqlParameter("Id", model.Id);
            var nameParameter = new SqlParameter("Name", model.Name);
            var amountParameter = new SqlParameter("Amount", model.Amount);
            var payDayParameter = new SqlParameter("PayDay", model.PayDay);

            var task = _dataAccess.ExecuteScalar<int>("up_UpdateUser", userIdParameter, nameParameter, amountParameter, payDayParameter);
            return task;
        }

        public async Task<UserModel> GetUser(Guid id)
        {
            UserModel model = new UserModel();
            var userIdParameter = new SqlParameter("Id", id);

            var reader =  await _dataAccess.ExecuteReader("up_GetUser", userIdParameter);

            while (reader.Read())
            {
                model.Amount = decimal.Parse(reader["Amount"].ToString());
                model.Id = (Guid) reader["Id"];
                model.Name =reader["Name"].ToString();
                model.PayDay = (int) reader["PayDay"];
            }

            return model;
        }
    }
}