using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int? ConfirmedById { get; set; }
        public Person? ConfirmedBy { get; set; }
        public int? IssuerId { get; set; }
        public Person? Issuer { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime DateToRelease { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime DateToEP { get; set; }
        public int? ReceiverId { get; set; }
        public Person? Receiver { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderState State { get; set; }
        public bool IsActive { get; set; }
    }

    public enum OrderState
    {
        NOWY, W_TRAKCIE, ZAKONCZONY
    }

}
