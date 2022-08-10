using NStore.Common;
using System;
using NStore.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NStore.ApiRequest
{
    public class SlideApiRequest
    {
        public static int CreateSlideRequest(SlideModel.Output.CreateSlide request)
        {
            var thanhvien = Common.AppContext.Current.Session.Get<MemberModel.Output.MemberInfo>("ThanhVien");
            var nhanvien = Common.AppContext.Current.Session.Get<UserModel.Output.UserInfo>("NhanVien");

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
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Url) ? "" : request.Url.ToString()), "url");
            requestContent.Add(new StringContent(request.Active.ToString()), "active");

            var response = client.PostAsync($"/api/Slide/CreateSlide", requestContent).Result;
            int result = 0;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadFromJsonAsync<int>().Result;
            }
            return result;
        }

        public static int UpdateSlideRequest(SlideModel.Output.UpdateSlide request)
        {
            var thanhvien = Common.AppContext.Current.Session.Get<MemberModel.Output.MemberInfo>("ThanhVien");
            var nhanvien = Common.AppContext.Current.Session.Get<UserModel.Output.UserInfo>("NhanVien");

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
            requestContent.Add(new StringContent(request.Id.ToString()), "id");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Url) ? "" : request.Url.ToString()), "url");
            requestContent.Add(new StringContent(request.Active.ToString()), "active");

            var response = client.PostAsync($"/api/Slide/UpdateSlide", requestContent).Result;
            int result = 0;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadFromJsonAsync<int>().Result;
            }
            return result;
        }
    }
}