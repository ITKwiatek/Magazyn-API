using Magazyn_API.Model.Excel.PL_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel
{
    public class ExcelOrderItemPL_1 : IExcelOrderItem, ITypePL_1
    {
        public IExcelComponent ExcelComponent { get; set; }
        public int Count { get; set; }
        public Cell CountCell { get; set; } = new(6, 7);
    }
}
