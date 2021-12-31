using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class Release
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }
        public int? ReceiverId { get; set; }
        public Person? Receiver { get; set; }
        public DateTime ReceiveDate { get; set; }
        public List<ReleaseItem> ReleaseItems { get; set; } = new();
    }
}
