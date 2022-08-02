using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class SlideModel
    {
        public class SlideBannerBase
        {
            public int Id { get; set; }

            [Display(Name = "Tên banner")]
            [Required(ErrorMessage = "Tên phải khác rỗng")]
            public string Name { get; set; }

            [Display(Name = "Hình banner")]
            public string Picture { get; set; }

            [Display(Name = "Liên kết (URL)")]
            public string Url { get; set; }

            [Display(Name = "Kích hoạt")]
            public bool Active { get; set; }
        }

        public class Input
        {
            public class SlideInfo : SlideBannerBase
            { }

            public class ListSlide
            {
                public bool Quantri { get; set; }
            }

            public class ReadSlideInfo
            {
                public int Id { get; set; }
            }

            public class DeleteSlide
            {
                public int Id { get; set; }
            }
        }

        public class Output
        {
            public class SlideInfo : SlideBannerBase
            { }

            public class CreateSlide : SlideBannerBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }

            public class UpdateSlide : SlideBannerBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }

            public class DeleteSlide
            {
                public int Code { get; set; }
            }
        }
    }
}