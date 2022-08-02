using Newtonsoft.Json;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NStore.Common
{
    public class ProductApiRequest
    {
        public static int CreateProductRequest(ProductModel.Output.AddProduct request)
        {
            var thanhvien = AppContext.Current.Session.Get<MemberModel.Output.MemberInfo>("ThanhVien");
            var nhanvien = AppContext.Current.Session.Get<UserModel.Output.UserInfo>("NhanVien");

            HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Clear();
            if (thanhvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", thanhvien.AccessToken);
            else if (nhanvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nhanvien.AccessToken);
            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));

            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Details.ToString()), "details");

            var response = client.PostAsync($"/api/Products/Create", requestContent).Result;
            int result = 0;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadFromJsonAsync<int>().Result;
            }
            return result;
        }

        public static int UpdateProductRequest(ProductModel.Output.UpdateProduct request)
        {
            var thanhvien = AppContext.Current.Session.Get<MemberModel.Output.MemberInfo>("ThanhVien");
            var nhanvien = AppContext.Current.Session.Get<UserModel.Output.UserInfo>("NhanVien");

            HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Clear();
            if (thanhvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", thanhvien.AccessToken);
            else if (nhanvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nhanvien.AccessToken);
            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));

            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.ViewCount.ToString()), "viewCount");
            requestContent.Add(new StringContent(request.Details.ToString()), "details");

            var response = client.PostAsync($"/api/products/update/{request.Id}", requestContent).Result;
            int result = 0;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadFromJsonAsync<int>().Result;
            }
            return result;
        }

        public static async Task<ProductModel.Output.DeleteProduct> DeleteProduct(ProductModel.Input.DeleteProduct request)
        {
            var thanhvien = AppContext.Current.Session.Get<MemberModel.Output.MemberInfo>("ThanhVien");
            var nhanvien = AppContext.Current.Session.Get<UserModel.Output.UserInfo>("NhanVien");

            HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:5001");
            if (thanhvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", thanhvien.AccessToken);
            else if (nhanvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nhanvien.AccessToken);
            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsJsonAsync($"/api/products/delete/{request.Id}", request.Id);

            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ProductModel.Output.DeleteProduct>(body);
            }

            return new ProductModel.Output.DeleteProduct
            {
                IsSuccessed = false,
                Noteti = "Xóa sản phẩm không thành công"
            };
        }

        public static async Task<List<CategoryModel>> GetCategoryAsync()
        {
            var thanhvien = AppContext.Current.Session.Get<MemberModel.Output.MemberInfo>("ThanhVien");
            var nhanvien = AppContext.Current.Session.Get<UserModel.Output.UserInfo>("NhanVien");

            HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:5001");
            if (thanhvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", thanhvien.AccessToken);
            else if (nhanvien != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", nhanvien.AccessToken);
            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(ConstantValues.Category.GetAll);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var data = (List<CategoryModel>)JsonConvert.DeserializeObject(body, typeof(List<CategoryModel>));
                return data;
            }
            throw new Exception(body);
        }
    }
}