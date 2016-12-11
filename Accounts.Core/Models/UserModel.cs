using System;

namespace Accounts.Core.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int PayDay { get; set; }
        public Guid Id { get; set; }
    }
}