using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }
        public int ComponentId { get; set; }
        public ComponentModel Component { get; set; }
        public int RequiredQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public ReleaseItem ReleaseItem { get; set; }
    }
}
