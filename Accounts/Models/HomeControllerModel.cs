using System;

namespace Accounts.Models
{
    public class HomeControllerModel
    {
        public Guid Users { get; set; }
        public SelectListModel PaymentTypes { get; set; }
    }
}