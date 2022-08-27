using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProductService : IProduct
    {
        private readonly NStoreContext _context;
        private readonly IStorage _storage;
        private const string USER_CONTENT_FOLDER_NAME = "Picture/Products";

        public ProductService(NStoreContext context, IStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public ProductModel.Output.PagedResult GetAllPaging(ProductModel.Input.GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pi.IsDefault == true
                        select new { p, c, pic, pi };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword) || x.p.Name.Contains(request.Keyword));
            }

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId || x.c.ParentId == request.CategoryId);
            }

            //3. Paging
            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.p.DateCreated);

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductModel.ProductBase()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    ImagePath = x.pi.ImagePath
                }).ToList();

            var pagedResult = new ProductModel.Output.PagedResult()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public ProductModel.Output.DetailProduct GetById(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id.Equals(productId));

            var categories = (from c in _context.Categories
                              join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                              where pic.ProductId == productId
                              select c.Name).ToList();
            var image = _context.ProductImages.FirstOrDefault(x => x.ProductId.Equals(productId) && x.IsDefault == true);
            var imageExtant = _context.ProductImages.Where(x => x.ProductId.Equals(productId) && x.IsDefault == false).ToList();

            var result = new ProductModel.Output.DetailProduct()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories,
                ImagePath = image.ImagePath,
                ProductImages = imageExtant.Select(x => new ProductImageModel.ProductImageBase()
                {
                    Id = x.Id,
                    DateCreated = x.DateCreated,
                    FileSize = x.FileSize,
                    ImagePath = x.ImagePath,
                    IsDefault = x.IsDefault,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder,
                    ProductName = product.Name
                }).ToList()
            };

            return result;
        }

        public int Create(ProductModel.Output.AddProduct request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                Name = request.Name,
                Description = request.Description,
                Details = request.Details
            };
            // Save Image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return product.Id;
        }

        public int Update(ProductModel.Output.UpdateProduct request)
        {
            var product = _context.Products.Find(request.Id);
            if (product == null)
            {
                return 0;
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.OriginalPrice = request.OriginalPrice;
            product.Stock = request.Stock;
            product.ViewCount = request.ViewCount;
            product.Description = request.Description;
            product.Details = request.Details;

            // Save Image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = _context.ProductImages.FirstOrDefault(x => x.IsDefault == true && x.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            _context.SaveChanges();
            return product.Id;
        }

        public async Task<ProductModel.Output.DeleteProduct> Detele(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(x => x.ProductId.Equals(productId));
            if (product == null)
            {
                return new ProductModel.Output.DeleteProduct()
                {
                    IsSuccessed = false,
                    Noteti = "Can not find product!"
                };
            }

            if (orderDetail != null)
            {
                return new ProductModel.Output.DeleteProduct()
                {
                    IsSuccessed = false,
                    Noteti = "Customer ordered this product!"
                };
            }

            var pic = _context.ProductInCategories.FirstOrDefault(x => x.ProductId.Equals(productId));
            if (pic != null)
            {
                _context.ProductInCategories.Remove(pic);
            }

            var images = _context.ProductImages.Where(x => x.ProductId == productId);
            if (images != null)
            {
                foreach (var image in images)
                {
                    await _storage.DeleteFileAsync(image.ImagePath);
                    _context.ProductImages.Remove(image);
                }
            }

            var result = _context.Products.Remove(product);

            if (result != null)
            {
                await _context.SaveChangesAsync();
                return new ProductModel.Output.DeleteProduct()
                {
                    IsSuccessed = true
                };
            }

            return new ProductModel.Output.DeleteProduct()
            {
                IsSuccessed = false,
                Noteti = "Delete product fail!"
            };
        }

        public ProductModel.Output.CategoryAssign CategoryAssign(int id, ProductModel.Output.CategoryAssign request)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return new ProductModel.Output.CategoryAssign()
                {
                    IsSuccessed = false,
                    Noteti = $"Can not find product id = {id}"
                };
            }

            foreach (var category in request.Categories)
            {
                var productInCategory = _context.ProductInCategories.FirstOrDefault(x => x.CategoryId == category.Id && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected == true)
                {
                    _context.ProductInCategories.Add(new ProductInCategory()
                    {
                        CategoryId = category.Id,
                        ProductId = id
                    });
                }
            }

            _context.SaveChanges();

            return new ProductModel.Output.CategoryAssign()
            {
                IsSuccessed = true,
            };
        }

        public List<ProductModel.ProductBase> GetFeaturedProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pi.IsDefault == true
                        select new { p, pic, pi };

            var data = (query.OrderByDescending(x => x.p.ViewCount)).ToList().Take(take).Select(x => new ProductModel.ProductBase()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                DateCreated = x.p.DateCreated,
                Description = x.p.Description,
                Details = x.p.Details,
                ImagePath = x.pi.ImagePath
            }).ToList();

            return data;
        }

        public List<ProductModel.ProductBase> GetLatestProduct(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pi.IsDefault == true
                        select new { p, pic, pi };

            var data = (query.OrderByDescending(x => x.p.DateCreated)).ToList().Take(take).Select(x => new ProductModel.ProductBase()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                DateCreated = x.p.DateCreated,
                Description = x.p.Description,
                Details = x.p.Details,
                ImagePath = x.pi.ImagePath
            }).ToList();

            return data;
        }

        private string SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            _storage.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public int ViewCount(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id.Equals(productId));
            if (product != null)
            {
                product.ViewCount++;
            }
            return _context.SaveChanges();
        }

        public List<ProductImageModel.ProductImageBase> GetAllImage(ProductImageModel.Input.GetAllImageProduct request)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id.Equals(request.ProductId));
            var listImage = _context.ProductImages.Where(x => x.ProductId.Equals(request.ProductId)).ToList();

            var result = listImage.OrderBy(a => a.SortOrder).ToList().Select(x => new ProductImageModel.ProductImageBase()
            {
                SortOrder = x.SortOrder,
                ProductName = product.Name,
                ImagePath = x.ImagePath,
                DateCreated = x.DateCreated,
                IsDefault = x.IsDefault,
                FileSize = x.FileSize,
                ProductId = x.ProductId,
                Id = x.Id
            }).ToList();
            return result;
        }

        public ProductImageModel.ProductImageBase GetImageById(int imageId)
        {
            var image = _context.ProductImages.FirstOrDefault(x => x.Id.Equals(imageId));

            var productImage = new ProductImageModel.ProductImageBase()
            {
                Id = image.Id,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder,
            };

            return productImage;
        }

        public int AddImage(ProductImageModel.Output.AddImage request)
        {
            var listImage = _context.ProductImages.Where(x => x.ProductId.Equals(request.ProductId)).ToList().OrderBy(a => a.SortOrder).ToList();

            if (request.IsDefault == true)
            {
                var imageDefault = listImage.FirstOrDefault(x => x.IsDefault.Equals(true));
                if (imageDefault != null)
                    imageDefault.IsDefault = false;

                request.SortOrder = 1;

                for (int i = request.SortOrder - 1; i < listImage.Count; i++)
                {
                    listImage[i].SortOrder++;
                }
            }
            else
            {
                if (request.SortOrder == 1)
                {
                    var imageDefault = listImage.FirstOrDefault(x => x.IsDefault.Equals(true));
                    imageDefault.IsDefault = false;

                    request.IsDefault = true;

                    for (int i = request.SortOrder - 1; i < listImage.Count; i++)
                    {
                        listImage[i].SortOrder++;
                    }
                }
                else
                {
                    if (request.SortOrder > listImage.Count)
                    {
                        request.SortOrder = listImage.Count + 1;
                    }
                    else
                    {
                        for (int i = request.SortOrder - 1; i < listImage.Count; i++)
                        {
                            listImage[i].SortOrder++;
                        }
                    }
                }
            }

            var image = new ProductImage()
            {
                //Product = _context.Products.FirstOrDefault(x => x.Id.Equals(request.ProductId)),
                ProductId = request.ProductId,
                DateCreated = DateTime.Now,
                FileSize = request.ThumbnailImage.Length,
                ImagePath = this.SaveFile(request.ThumbnailImage),
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder
            };
            _context.ProductImages.Add(image);
            _context.SaveChanges();
            return image.ProductId;
        }

        public int UpdateImage(ProductImageModel.Output.UpdateImage request)
        {
            var listImage = _context.ProductImages.Where(x => x.ProductId.Equals(request.ProductId)).ToList().OrderBy(a => a.SortOrder).ToList();

            if (request.IsDefault == true)
            {
                var imageDefault = listImage.FirstOrDefault(x => x.IsDefault.Equals(true));
                imageDefault.IsDefault = false;

                request.SortOrder = 1;

                for (int i = request.SortOrder - 1; i < listImage.Count; i++)
                {
                    listImage[i].SortOrder++;
                }
            }
            else
            {
                if (request.SortOrder == 1)
                {
                    var imageDefault = listImage.FirstOrDefault(x => x.IsDefault.Equals(true));
                    imageDefault.IsDefault = false;

                    request.IsDefault = true;

                    for (int i = request.SortOrder - 1; i < listImage.Count; i++)
                    {
                        listImage[i].SortOrder++;
                    }
                }
                else
                {
                    if (request.SortOrder > listImage.Count)
                    {
                        request.SortOrder = listImage.Count + 1;
                    }
                    else
                    {
                        for (int i = request.SortOrder - 1; i < listImage.Count; i++)
                        {
                            listImage[i].SortOrder++;
                        }
                    }
                }
            }

            var image = _context.ProductImages.FirstOrDefault(x => x.Id.Equals(request.Id));

            if (image != null)
            {
                image.ProductId = request.ProductId;
                image.IsDefault = request.IsDefault;
                image.DateCreated = DateTime.Now;
                image.SortOrder = request.SortOrder;

                if (request.ThumbnailImage != null)
                {
                    image.FileSize = request.ThumbnailImage.Length;
                    image.ImagePath = this.SaveFile(request.ThumbnailImage);
                }

                _context.ProductImages.Update(image);
                _context.SaveChanges();
                return image.ProductId;
            };

            return 0;
        }

        public int DeleteImage(int imageId)
        {
            var image = _context.ProductImages.FirstOrDefault(x => x.Id.Equals(imageId));

            var listImages = _context.ProductImages.Where(x => x.ProductId.Equals(image.ProductId)).ToList().OrderBy(a => a.SortOrder).ToList();

            if (image == null)
            {
                return 0;
            }

            if (image.SortOrder > 1)
            {
                for (int i = image.SortOrder; i < listImages.Count; i++)
                {
                    listImages[i].SortOrder--;
                }
            }

            _storage.DeleteFileAsync(image.ImagePath);
            _context.ProductImages.Remove(image);

            var change = _context.SaveChanges();

            if (change > 0)
            {
                return image.ProductId;
            }

            return 0;
        }
    }
}