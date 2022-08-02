using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Identification { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
