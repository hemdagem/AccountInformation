using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Core.Models;

namespace Accounts.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<ListItem>> GetUsers();
        Task<Guid> AddUser(UserModel model);
        Task<int> UpdateUser(UserModel model);
        Task<UserModel> GetUser(Guid id);
    }
}