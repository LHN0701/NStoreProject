using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class PromotionModel
    {
        public class PromotionBase
        {
            public int Id { get; set; }

            public DateTime FromDate { get; set; }

            public DateTime ToDate { get; set; }

            public bool ApplyForAll { get; set; }

            public int DiscountPercent { get; set; }

            public int DiscountAmount { get; set; }

            public string Name { get; set; }
        }

        public class Input
        {
            public class GetPromotionClient
            {
                public string Name { get; set; }
                public int IdMember { get; set; }
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