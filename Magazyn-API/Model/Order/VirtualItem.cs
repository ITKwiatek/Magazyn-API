using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class VirtualItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int VirtualOrderId { get; set; }
        public VirtualOrder VirtualOrder { get; set; }
        public int ComponentId { get; set; }
        public ComponentModel Component { get; set; }
        public int RequiredQuantity { get; set; }
    }
}
