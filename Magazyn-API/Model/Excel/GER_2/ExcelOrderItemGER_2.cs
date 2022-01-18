using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel.GER_2
{
    public class ExcelOrderItemGER_2 : IExcelOrderItem
    {
        public IExcelComponent ExcelComponent { get; set; }
        public int Count { get; set; }
        public Cell CountCell { get; set; } = new(6, 3);
    }
}
