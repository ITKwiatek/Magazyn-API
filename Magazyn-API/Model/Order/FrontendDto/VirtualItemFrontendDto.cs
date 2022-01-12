using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class VirtualItemFrontendDto
    {
        public int Id { get; set; }
        public int VirtualOrderId { get; set; }
        public ComponentModel Component { get; set; }
        public int RequiredQuantity { get; set; }
    }
}
