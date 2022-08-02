using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class HomeModel
    {
        public List<SlideModel.Output.SlideInfo> Slides { get; set; }
        public List<ProductModel.ProductBase> FeaturedProducts { get; set; }
        public List<ProductModel.ProductBase> LatestProducts { get; set; }
    }
}