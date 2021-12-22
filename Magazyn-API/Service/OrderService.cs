using Magazyn_API.Data;
using Magazyn_API.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Service
{
    public class OrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }
        public OrderState UpdateState(int orderId)
        {
            OrderState state;
            int finishedCount = 0;
            List<OrderItem> items = _repo.GetItemsByOrderId(orderId);
            foreach(var item in items)
            {
                if (item.CurrentQuantity == item.RequiredQuantity)
                    finishedCount++;
            }
            if (finishedCount == 0)
                state = OrderState.NOWY;
            else if (finishedCount == items.Count)
                state = OrderState.ZAKONCZONY;
            else
                state = OrderState.W_TRAKCIE;

            return state;
        }
    }
}
