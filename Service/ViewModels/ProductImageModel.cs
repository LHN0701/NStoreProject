using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class ProductImageModel
    {
        public class ProductImageBase
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }

            public string ImagePath { get; set; }
            public bool IsDefault { get; set; }
            public DateTime DateCreated { get; set; }
            public int SortOrder { get; set; }
            public long FileSize { get; set; }
        }

        public class Input
        {
            public class GetAllImageProduct
            {
                public int ProductId { get; set; }
            }

            public class DeleteImage
            {
                public int ImageId { get; set; }
            }
        }

        public class Output
        {
            public class AddImage : ProductImageBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }

            public class UpdateImage : ProductImageBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }
        }
    }
}