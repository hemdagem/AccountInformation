using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Core.Models;

namespace Accounts.Core.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<PaymentModel>> GetPaymentsById(Guid userId);
        Task<Guid> AddPayment(PaymentModel model);
        Task<int> DeletePayment(Guid guid);
        Task<int> UpdatePayment(PaymentModel model);
        Task<List<ListItem>> GetPaymentTypes();
    }
}