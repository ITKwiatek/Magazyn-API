using Magazyn_API.Model.Order.FrontendDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class VirtualOrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatedById { get; set; }
        public Person? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderModel> Orders { get; set; } = new();
        public List<VirtualItem> OrderItems { get; set; } = new();
    }
}
