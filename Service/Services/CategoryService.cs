using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CategoryService : ICategory
    {
        private readonly NStoreContext _context;

        public CategoryService(NStoreContext context)
        {
            _context = context;
        }

        public GetAllCategory GetAll()
        {
            var parent = _context.Categories.Where(x => x.ParentId == null);
            var child = _context.Categories.Where(x => x.ParentId.HasValue);
            var result = new GetAllCategory()
            {
                CategoryParent = parent.Select(x => new CategoryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
                }).ToList(),

                CategoryChild = child.Select(x => new CategoryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
                }).ToList(),
            };
            return result;
        }

        public Category GetById(int id)
        {
            var result = _context.Categories.FirstOrDefault(x => x.Id.Equals(id));

            return result;
        }

        public CategoryModel GetProductCategory(int id)
        {
            var productCategory = _context.ProductInCategories.FirstOrDefault(x => x.ProductId.Equals(id));

            if (productCategory == null)
            {
                return new CategoryModel();
            }

            var category = _context.Categories.FirstOrDefault(x => x.Id.Equals(productCategory.CategoryId));

            return new CategoryModel()
            {
                Id = productCategory.CategoryId,
                Name = category.Name,
                ParentId = category.ParentId
            };
        }
    }
}