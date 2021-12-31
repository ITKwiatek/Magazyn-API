using Magazyn_API.Data;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Mappers
{
    public class FrontendMapper
    {
        private readonly IOrderRepository _repo;

        public FrontendMapper(IOrderRepository repo)
        {
            _repo = repo;
        }
        public OrderCard OrderCard(OrderModel order)
        {
            OrderCard card = new();
            card.DateToRelease = order.DateToRelease;
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
            if (person == null)
                return new PersonInfo();

            var info = new PersonInfo();
            info.Id = person.Id;
            info.Name = person.Name;
            info.Surname = person.Surname;
            return info;
        }
        #region Original --> FrontendDto
        public OrderModelFrontendDto OrderModelFrontendDto(OrderModel order)
        {
            OrderModelFrontendDto dto = new();
            dto.Id = order.Id;
            dto.ConfirmedBy = PersonInfo(order.ConfirmedBy);
            dto.DateToRelease = order.DateToRelease;
            dto.DateToWarehouse = order.DateToWarehouse;
            dto.Device = order.Device;
            dto.IsActive = order.IsActive;
            dto.Issuer = PersonInfo(order.Issuer);
            dto.Items = ItemsDto(order.OrderItems);
            dto.State = order.State;

            return dto;
        }

        public List<OrderItemFrontendDto> ItemsDto(List<OrderItem> orderItems)
        {
            List<OrderItemFrontendDto> dtos = new();
            foreach(var oItem in orderItems)
            {
                dtos.Add(ItemDto(oItem));
            }
            return dtos;
        }

        public OrderItemFrontendDto ItemDto(OrderItem orderItem)
        {
            OrderItemFrontendDto dto = new();
            dto.Component = orderItem.Component;
            dto.ComponentId = orderItem.ComponentId;
            dto.CurrentQuantity = orderItem.CurrentQuantity;
            dto.Id = orderItem.Id;
            dto.OrderId = orderItem.OrderId;
            dto.RequiredQuantity = orderItem.RequiredQuantity;
            return dto;
        }
        #region Virtual
        public List<VirtualOrderCard> VirtualOrdersCards(List<VirtualOrderModel> models)
        {
            List<VirtualOrderCard> dtos = new();
            foreach(var model in models)
            {
                dtos.Add(VirtualOrderCard(model));
            }

            return dtos;
        }
        public VirtualOrderCard VirtualOrderCard(VirtualOrderModel model)
        {
            VirtualOrderCard dto = new();
            dto.CreatedBy = PersonInfo(model.CreatedBy);
            dto.CreatedDate = model.CreatedDate;
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.OrdersIds = OrderIds(model.Orders);

            return dto;
        }
        //public List<VirtualOrderFrontendDto> VirtualOrdersFrontendDtos(List<VirtualOrderModel> models)
        //{
        //    List<VirtualOrderFrontendDto> dtos = new();
        //    foreach(var model in models)
        //    {
        //        dtos.Add(VirtualOrderFrontendDto(model));
        //    }

        //    return dtos;
        //}
        public VirtualOrderFrontendDto VirtualOrderFrontendDto(VirtualOrderModel model)
        {
            VirtualOrderFrontendDto dto = new();
            dto.CreatedBy = model.CreatedBy;
            dto.CreatedDate = model.CreatedDate;
            dto.Id = model.Id;
            dto.Items = VirtualItemsFrontendDtos(model.OrderItems);
            dto.Name = model.Name;
            dto.OrdersIds = OrderIds(model.Orders);
            return dto;
        }
        public List<int> OrderIds(List<OrderModel> models)
        {
            List<int> dtos = new();
            foreach(var model in models)
            {
                dtos.Add(model.Id);
            }

            return dtos;
        }
        public List<VirtualItemFrontendDto> VirtualItemsFrontendDtos(List<VirtualItem> items)
        {
            List<VirtualItemFrontendDto> dtos = new();
            foreach (var item in items)
            {
                dtos.Add(VirtualItemFrontendDto(item));
            }

            return dtos;
        }
        public VirtualItemFrontendDto VirtualItemFrontendDto(VirtualItem item)
        {
            VirtualItemFrontendDto dto = new();
            dto.Component = item.Component;
            dto.Id = item.Id;
            dto.RequiredQuantity = item.RequiredQuantity;
            dto.VirtualOrderId = item.VirtualOrderId;

            return dto;
        }
        #endregion Virtual
        #endregion Original --> FrontendDto
        #region FrontendDto --> Original
        public OrderModel OrderModel(OrderModelFrontendDto orderDto)
        {
            OrderModel order = _repo.GetOrderWithItemsById(orderDto.Id);
            order.ConfirmedBy = Person(orderDto.ConfirmedBy);
            order.DateToRelease = orderDto.DateToRelease;
            order.DateToWarehouse = orderDto.DateToWarehouse;
            order.IsActive = orderDto.IsActive;
            order.Issuer = Person(orderDto.Issuer);
            order.OrderItems = OrderItems(orderDto.Items);
            order.State = orderDto.State;
            return order;
        }

        public List<OrderItem> OrderItems(List<OrderItemFrontendDto> itemsDto)
        {
            List<OrderItem> items = new List<OrderItem>();
            foreach(var itemDto in itemsDto)
            {
                OrderItem item = OrderItem(itemDto);
                items.Add(item);
            }
            return items;
        }

        public OrderItem OrderItem(OrderItemFrontendDto itemDto)
        {
            OrderItem item = _repo.GetItemById(itemDto.Id);
            item.CurrentQuantity = itemDto.CurrentQuantity;

            return item;
        }

        public Person Person(PersonInfo personInfo)
        {
            Person person = new Person();
            person = _repo.GetPersonById(person.Id);
            return person;
        }


        #endregion FrontendDto --> Original
    }
}
