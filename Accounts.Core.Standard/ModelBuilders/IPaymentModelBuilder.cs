using System.Collections.Generic;
using System.Data;
using Accounts.Core.Models;

namespace Accounts.Core.ModelBuilders
{
    public interface IPaymentModelBuilder
    {
        List<PaymentModel> Build(IDataReader reader);
    }
}