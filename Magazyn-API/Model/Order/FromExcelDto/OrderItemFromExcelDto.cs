using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FromExcelDto
{
    public class OrderItemFromExcelDto
    {
        public ComponentModelFromExcelDto Component { get; set; }
        public int Quantity { get; set; }
    }
}
