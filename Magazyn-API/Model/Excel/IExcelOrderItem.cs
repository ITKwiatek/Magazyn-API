using Magazyn_API.Model.Order;

namespace Magazyn_API.Model.Excel
{
    public interface IExcelOrderItem
    {
        public IExcelComponent ExcelComponent { get; set; }
        public int Count { get; set; }
        public Cell CountCell { get; set; }
    }
}