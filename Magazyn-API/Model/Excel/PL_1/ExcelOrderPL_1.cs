using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Excel.PL_1
{
    public class ExcelOrderPL_1 : IExcelOrderDates
    {
        public string ProjectName { get; set; }
        public string DeviceName { get; set; }
        public string GroupName { get; set; }
        public List<IExcelOrderItem> OrderItems { get; set; }
        public DateTime DateToEP { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public Cell ProjectNameCell { get; set; } = new(1, 3);
        public Cell DeviceNameCell { get; set; } = new(1, 4);
        public Cell DateToEPCell { get; set; } = new(2, 4);
        public Cell DateToWarehouseCell { get; set; } = new(3, 4);
        public Cell ReleaseDateCell { get; set; } = new(4, 4);
    }
}
