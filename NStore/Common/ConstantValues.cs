using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Common
{
    public class ConstantValues
    {
        public class Home
        {
            public const string Slides = "api/Slide/ListSlide";
            public const string FeatureProduct = "api/products/featured";
            public const string LatestProduct = "api/products/latest";
        }

        public class Product
        {
            public const string Paging = "api/Products/paging";
            public const string Detail = "api/Products/Detail";
            public const string ViewCount = "api/Products/ViewCount";
            public const string CategoryAssign = "api/Products/CategoryAssign";
            public const string ProductCategory = "api/Categories/ProductCategory";
            public const string Update = "api/Products";
            public const string Delete = "api/Products/Delete";
            public const string GetAllImage = "api/Products/GetAllImage";
            public const string GetImageById = "api/Products/GetImageById";
            public const string AddImage = "api/Products/AddImage";
            public const string DeleteImage = "api/Products/DeleteImage";
        }

        public class Category
        {
            public const string GetAll = "api/Categories/GetAll";
            public const string GetById = "api/Categories/GetById";
        }

        public class User
        {
            public const string GetAllUser = "api/Users/GetAllUser";
            public const string GetById = "api/Users/GetById";
            public const string Login = "api/Users/Login";
            public const string Logout = "api/Users/Logout";
            public const string ChangePassword = "api/Users/ChangePassword";
            public const string CreateUser = "api/Users/CreateUser";
            public const string UpdateUser = "api/Users/UpdateUser";
            public const string DeleteUser = "api/Users/DeleteUser";
        }

        public class Member
        {
            public const string AnonymousLogin = "api/Member/AnonymousLogin";

            public const string Login = "api/Member/Login";
            public const string Logout = "api/Member/Logout";
            public const string Register = "api/Member/Register";
            public const string ConfirmEmail = "api/Member/ConfirmEmail";
            public const string CheckEmail = "api/Member/CheckEmail";
            public const string ChangePassword = "api/Member/ChangePassword";
        }

        public class Slide
        {
            public const string ListSlide = "api/Slide/ListSlide";
            public const string SlideInfo = "api/Slide/Detail";
            public const string DeleteSlide = "api/Slide/DeleteSlide";
        }

        public class Order
        {
            public const string CreateOrder = "api/Orders/CreateOrder";
        }

        public class Promotion
        {
            public const string GetAll = "api/Promotion/GetAll";
            public const string GetById = "api/Promotion/GetById";
            public const string GetPromotionClient = "api/Promotion/GetPromotionClient";
            public const string CreatePromotion = "api/Promotion/CreatePromotion";
            public const string UpdatePromotion = "api/Promotion/UpdatePromotion";
            public const string DeletePromotion = "api/Promotion/DeletePromotion";
        }
    }
}