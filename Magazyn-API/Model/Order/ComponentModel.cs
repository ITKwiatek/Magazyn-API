using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class ComponentModel : IComponentModel
    {

        [Key, ForeignKey("OrderItem")]
        public int Id { get; set; }
        [MaxLength(30)]
        public string ArticleNumber { get; set; }
        [MaxLength(30)]
        public string OrderingNumber { get; set; }
        [MaxLength(35)]
        public string Supplier { get; set; }
        [MaxLength(30)]
        public string SAP { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
