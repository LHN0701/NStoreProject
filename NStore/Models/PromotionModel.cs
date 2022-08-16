using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class PromotionModel
    {
        public class PromotionBase
        {
            [Display(Name = "Id")]
            public int Id { get; set; }

            [Required(ErrorMessage = "Please enter date start.")]
            [Display(Name = "FromDate")]
            public DateTime FromDate { get; set; }

            [Required(ErrorMessage = "Please enter date end.")]
            [Display(Name = "ToDate")]
            public DateTime ToDate { get; set; }

            [Display(Name = "ApplyForAll")]
            public bool ApplyForAll { get; set; }

            [Display(Name = "DiscountPercent")]
            [Required(ErrorMessage = "Please enter discount percent.")]
            public int DiscountPercent { get; set; }

            [Display(Name = "DiscountAmount")]
            [Required(ErrorMessage = "Please enter discount amount.")]
            public int DiscountAmount { get; set; }

            [Display(Name = "Code sale")]
            [Required(ErrorMessage = "Please enter code sale.")]
            public string Name { get; set; }
        }

        public class Input
        {
            public class GetPromotionClient
            {
                public string Name { get; set; }
                public int IdMember { get; set; }
            }

            public class UpdatePromotion
            {
                public int Id { get; set; }
            }

            public class DeletePromotion
            {
                public int Id { get; set; }
            }
        }

        public class Output
        {
            public class GetPromotionClient : NotetiModel
            {
                public int DiscountPercent { get; set; }
                public int Id { get; set; }
            }
        }
    }
}