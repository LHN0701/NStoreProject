﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }

    public class GetAllCategory
    {
        public List<CategoryModel> CategoryParent { get; set; }
        public List<CategoryModel> CategoryChild { get; set; }
    }
}