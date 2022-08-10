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

            [Display(Name = "Name banner")]
            [Required(ErrorMessage = "Name must be non-empty.")]
            public string Name { get; set; }

            [Display(Name = "Picture banner")]
            public string Picture { get; set; }

            [Display(Name = "URL")]
            public string Url { get; set; }

            [Display(Name = "Active")]
            [Required(ErrorMessage = "Active must be non-empty.")]
            public bool Active { get; set; }
        }

        public class Input
        {
            public class SlideInfo : SlideBannerBase
            { }

            public class ListSlide
            {
                public bool Manage { get; set; }
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
                [Display(Name = "ThumbnailImage")]
                [Required(ErrorMessage = "ThumbnailImage must be non-empty.")]
                public IFormFile ThumbnailImage { get; set; }
            }

            public class UpdateSlide : SlideBannerBase
            {
                [Display(Name = "ThumbnailImage")]
                public IFormFile ThumbnailImage { get; set; }
            }

            public class DeleteSlide
            {
                public int Code { get; set; }
            }
        }
    }
}