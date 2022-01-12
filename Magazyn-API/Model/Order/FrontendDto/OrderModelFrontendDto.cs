using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class OrderModelFrontendDto
    {
        public int Id { get; set; }
        public DeviceFrontendDto Device { get; set; }
        public PersonInfo? ConfirmedBy { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime DateToRelease { get; set; }
        public List<OrderItemFrontendDto> Items { get; set; } = new List<OrderItemFrontendDto>();
        public OrderState State { get; set; }
        public bool IsActive { get; set; }
    }
}
