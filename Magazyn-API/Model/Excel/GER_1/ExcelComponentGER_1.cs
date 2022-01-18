using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel.GR_1
{
    public class ExcelComponentGER_1 : IExcelComponent
    {
        public string SAP { get; set; }
        public string Supplier { get; set; }
        public string ArticleNumber { get; set; }
        public string OrderingNumber { get; set; }
        public string Description { get; set; }
        public Cell SapCell { get; set; } = new(6, 1);
        public Cell SupplierCell { get; set; } = new(6, 7);
        public Cell ArticleNumberCell { get; set; } = new(6, 5);
        public Cell OrderingNumberCell { get; set; } = new(6, 6);
        public Cell DescriptonCell { get; set; } = new(6, 4);
    }
}
