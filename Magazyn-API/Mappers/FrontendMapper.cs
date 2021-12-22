using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Mappers
{
    public class FrontendMapper
    {
        public OrderCard OrderCard(OrderModel order)
        {
            OrderCard card = new();
            card.DateRelease = order.DateRelease;
            card.DateToWarehouse = order.DateToWarehouse;
            card.DeviceName = order.Device.Name;
            card.Id = order.Id;
            card.Receiver = PersonInfo(order.Receiver);
            card.State = order.State;
            card.FinishedItemsCount = getFinishedCount();
            card.ItemsCount = order.OrderItems.Count;

            return card;

            int getFinishedCount()
            {
                int count = 0;
                foreach(var i in order.OrderItems)
                {
                    if (i.CurrentQuantity == i.RequiredQuantity)
                        count++;
                }
                return count;
            }
        }

        public List<OrderCard> OrderCards(List<OrderModel> orders)
        {
            List<OrderCard> cards = new();
            foreach(var order in orders)
            {
                cards.Add(OrderCard(order));
            }
            return cards;
        }

        public PersonInfo PersonInfo(Person person)
        {
            var info = new PersonInfo();
            info.Name = person.Name;
            info.Surname = person.Surname;
            return info;
        }
    }
}
