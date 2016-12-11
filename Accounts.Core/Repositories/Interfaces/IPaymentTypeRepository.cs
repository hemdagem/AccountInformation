using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Core.Models;

namespace Accounts.Core.Repositories.Interfaces
{
    public interface IPaymentTypeRepository
    {
        Task<List<ListItem>> GetPaymentTypes();
        Task<Guid> AddPaymentType(PaymentTypes model);
        Task<int> UpdatePaymentType(PaymentTypes model);
        Task<PaymentTypes> GetPaymentType(Guid id);
    }
}