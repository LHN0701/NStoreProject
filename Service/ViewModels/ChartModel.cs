using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class ChartModel
    {
        public class OneMonTotalSale
        {
            public int Month { get; set; }
            public decimal TotalSaleOfMon { get; set; }
        }

        public class ExpenseAndSale
        {
            public int Month { get; set; }
            public decimal TotalSaleOfMon { get; set; }
            public decimal TotalExpense { get; set; }
        }
    }
}