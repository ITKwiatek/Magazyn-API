using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class ReleaseItem
    {
        [Key]
        public int ReleaseId { get; set; }
        public Release Release { get; set; }
        [Key]
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
        public int Quantity { get; set; }
    }
}
