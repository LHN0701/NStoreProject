using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class ProductModel
    {
        public class ProductBase
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Please enter product name")]
            public string Name { get; set; }

            public decimal Price { get; set; }
            public decimal OriginalPrice { get; set; }
            public int Stock { get; set; }
            public int ViewCount { get; set; }
            public DateTime DateCreated { get; set; }
            public string Description { get; set; }
            public string Details { get; set; }
            public string ImagePath { get; set; }
            public List<string> Categories { get; set; } = new List<string>();
        }

        public class Input
        {
            public class GetProductBase
            {
                public int PageIndex { get; set; }
                public int PageSize { get; set; }
            }

            public class GetProductPagingRequest : GetProductBase
            {
                public string Keyword { get; set; }
                public int? CategoryId { get; set; }
            }

            public class DeleteProduct
            {
                public int Id { get; set; }
            }

            public class CategoryAssign
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int? ParentId { get; set; }
                public bool Selected { get; set; }
            }
        }

        public class Output
        {
            public class Base
            {
                public bool IsSuccessed { get; set; }
                public string Noteti { get; set; }
            }

            public class PagedResultBase
            {
                public int PageIndex { get; set; }

                public int PageSize { get; set; }

                public int TotalRecords { get; set; }

                public int PageCount
                {
                    get
                    {
                        var pageCount = (float)TotalRecords / PageSize;
                        return (int)Math.Ceiling(pageCount);
                    }
                }
            }

            public class PagedResult : PagedResultBase
            {
                public List<ProductBase> Items { set; get; }
            }

            public class AddProduct : ProductBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }

            public class UpdateProduct : ProductBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }

            public class DeleteProduct : Base
            {
            }

            public class CategoryAssign : Base
            {
                public int Id { get; set; }
                public List<ProductModel.Input.CategoryAssign> Categories { get; set; }
            }

            public class ProductCategoryViewModel
            {
                public CategoryBase Category { get; set; }
                public PagedResult Result { set; get; }
            }
        }
    }
}