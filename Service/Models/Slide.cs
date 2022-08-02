using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Slide
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
    }
}
