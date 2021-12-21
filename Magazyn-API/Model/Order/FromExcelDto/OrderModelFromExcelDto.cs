using Magazyn_API.Model.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FromExcelDto
{
    public class OrderModelFromExcelDto
    {
        public string ProjectName { get; set; }
        public string DeviceName { get; set; }
        public DateTime DateToEP { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<OrderItemFromExcelDto> OrderItems { get; set; } = new List<OrderItemFromExcelDto>();
    }
}
