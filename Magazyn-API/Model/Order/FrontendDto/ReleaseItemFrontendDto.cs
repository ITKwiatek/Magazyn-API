using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class ReleaseItemFrontendDto
    {
        public int ReleaseId { get; set; }
        public OrderItemFrontendDto OrderItem { get; set; }
        public int Quantity { get; set; }
    }
}
