using Magazyn_API.Model.Auth;
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
        public string? ConfirmedById { get; set; }
        public ApplicationUser? ConfirmedBy { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime DateToRelease { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime DateToEP { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderState State { get; set; }
        public bool IsActive { get; set; }
        public List<VirtualOrder> VirtualOrders { get; set; }
    }

    public enum OrderState
    {
        NOWY, W_TRAKCIE, ZAKONCZONY
    }

}
