using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Promotion
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool ApplyForAll { get; set; }
        public int DiscountPercent { get; set; }
        public int DiscountAmount { get; set; }
        public string Name { get; set; }
    }
}
