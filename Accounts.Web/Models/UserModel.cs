using System;
using System.ComponentModel.DataAnnotations;

namespace Accounts.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int PayDay { get; set; }
    }
}