using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class SlideModel
    {
        public class SlideBannerBase
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Picture { get; set; }

            public string Url { get; set; }

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
                public IFormFile ThumbnailImage { get; set; }
            }

            public class UpdateSlide : SlideBannerBase
            {
                public IFormFile ThumbnailImage { get; set; }
            }
        }
    }
}