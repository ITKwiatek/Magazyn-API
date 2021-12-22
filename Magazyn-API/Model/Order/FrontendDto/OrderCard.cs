using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class OrderCard
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public DateTime DateToWarehouse { get; set; }
        public DateTime DateRelease { get; set; }
        public PersonInfo? Receiver { get; set; }
        public int ItemsCount { get; set; }
        public int FinishedItemsCount { get; set; }
        public OrderState State { get; set; }
    }
}
