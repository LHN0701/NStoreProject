using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Member
    {
        public Member()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? Gender { get; set; }
        public bool? Active { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string AccountFrom { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
