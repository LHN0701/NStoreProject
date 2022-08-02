using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Category
    {
        public Category()
        {
            ProductInCategories = new HashSet<ProductInCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<ProductInCategory> ProductInCategories { get; set; }
    }
}
