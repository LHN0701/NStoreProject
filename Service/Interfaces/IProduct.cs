using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.ViewModels;

namespace Service.Interfaces
{
    public interface IProduct
    {
        ProductModel.Output.PagedResult GetAllPaging(ProductModel.Input.GetProductPagingRequest request);

        ProductModel.Output.DetailProduct GetById(int productId);

        int ViewCount(int productId);

        int Create(ProductModel.Output.AddProduct request);

        int Update(ProductModel.Output.UpdateProduct request);

        Task<ProductModel.Output.DeleteProduct> Detele(int productId);

        ProductModel.Output.CategoryAssign CategoryAssign(int id, ProductModel.Output.CategoryAssign request);

        List<ProductModel.ProductBase> GetFeaturedProducts(int take);

        List<ProductModel.ProductBase> GetLatestProduct(int take);

        List<ProductImageModel.ProductImageBase> GetAllImage(ProductImageModel.Input.GetAllImageProduct request);

        ProductImageModel.ProductImageBase GetImageById(int imageId);

        int AddImage(ProductImageModel.Output.AddImage request);

        int UpdateImage(ProductImageModel.Output.UpdateImage request);

        int DeleteImage(int imageId);
    }
}