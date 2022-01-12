using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class VirtualManyToMany
    {
        [Key]
        public int OrderId { get; set; }
        [Key]
        public int VirtualOrderId { get; set; }
        public OrderModel Order { get; set; }
        public VirtualOrder VirtualOrder { get; set; }
    }
}
