using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel.GR_1
{
    public class ExcelOrderItemGER_1 : IExcelOrderItem
    {
        public IExcelComponent ExcelComponent { get; set; }
        public int Count { get; set; }
        public Cell CountCell { get; set; } = new(6, 3);
    }
}
