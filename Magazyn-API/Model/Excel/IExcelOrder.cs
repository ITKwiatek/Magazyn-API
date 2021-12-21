using Magazyn_API.Model.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Mappers
{
    public interface IExcelOrder
    {
        public string ProjectName { get; set; }
        public string DeviceName { get; set; }
        public DateTime DateToEP { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Cell DateToEPCell { get; set; }
        public Cell DateToWarehouseCell { get; set; }
        public Cell ReleaseDateCell { get; set; }
        public Cell ProjectNameCell { get; set; }
        public Cell DeviceNameCell { get; set; }
        public List<IExcelOrderItem> OrderItems { get; set; }
    }
}
