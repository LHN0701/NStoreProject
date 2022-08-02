using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }

    public class CategoryBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public int? ParentId { get; set; }
    }
}