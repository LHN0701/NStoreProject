using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
            ProductImages = new HashSet<ProductImage>();
            ProductInCategories = new HashSet<ProductInCategory>();
        }

        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductInCategory> ProductInCategories { get; set; }
    }
}
