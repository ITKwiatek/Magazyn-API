using Magazyn_API.Model.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class Release : IRelease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }
        public string? ReceiverId { get; set; }
        public ApplicationUser? Receiver { get; set; }
        public string IssuerId { get; set; }
        public ApplicationUser Issuer { get; set; }
        public DateTime ReleasedDate { get; set; }
        public List<ReleaseItem> ReleaseItems { get; set; } = new();
        public string ReceiverInfo { get; set; }
    }
}
