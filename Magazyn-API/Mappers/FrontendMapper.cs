using Magazyn_API.Data;
using Magazyn_API.Model.Auth;
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

        public PersonInfo PersonInfo(ApplicationUser person)
        {
            if (person == null)
                return new PersonInfo();

            var info = new PersonInfo();
            info.Id = person.Id;
            info.FirstName = person.FirstName;
            info.Surname = person.Surname;
            return info;
        }
        #region Original --> FrontendDto
        #region Device
        public DeviceFrontendDto DeviceFrontendDto(Device model)
        {
            DeviceFrontendDto dto = new();
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.Project = ProjectFrontendDto(model.Project);

            return dto;
        }

        public ProjectFrontendDto ProjectFrontendDto(Project model)
        {
            ProjectFrontendDto dto = new();
            dto.Id = model.Id;
            dto.Name = model.Name;
            return dto;
        }
        #endregion Device
        public OrderModelFrontendDto OrderModelFrontendDto(OrderModel order)
        {
            OrderModelFrontendDto dto = new();
            dto.Id = order.Id;
            dto.ConfirmedBy = PersonInfo(order.ConfirmedBy);
            dto.DateToRelease = order.DateToRelease;
            dto.DateToWarehouse = order.DateToWarehouse;
            dto.Device = DeviceFrontendDto(order.Device);
            dto.IsActive = order.IsActive;
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
        #region Release
        public ReleaseFrontendDto ReleaseFrontendDto(Release model)
        {
            ReleaseFrontendDto dto = new();

            if (model != null)
            {
                dto.Device = DeviceFrontendDto(_repo.GetOrderWithItemsById(model.OrderId).Device);
                dto.Id = model.Id;
                dto.Issuer = PersonInfo(model.Issuer);
                dto.OrderId = model.OrderId;
                dto.ReleasedDate = model.ReleasedDate;
                dto.ReleaseItems = ReleaseItemsFrontendDtos(model.ReleaseItems);
            }
            return dto;
        }
        public ReleaseCardFrontendDto ReleaseCardFrontendDto(Release model)
        {
            ReleaseCardFrontendDto dto = new();
            dto.Id = model.Id;
            dto.Issuer = PersonInfo(model.Issuer);
            dto.OrderId = model.OrderId;
            dto.ReleasedComponentsCount = ComponentCount(model.ReleaseItems);
            dto.ReleasedDate = model.ReleasedDate;
            dto.ReleasedItemsCount = model.ReleaseItems.Count;

            return dto;

            int ComponentCount(List<ReleaseItem> items)
            {
                int count = 0;
                foreach(var i in items)
                {
                    count += i.Quantity;
                }
                return count;
            }
        }
        public List<ReleaseCardFrontendDto> ReleaseCardsFrontendDto(List<Release> models)
        {
            List<ReleaseCardFrontendDto> dtos = new();
            foreach(var m in models)
            {
                dtos.Add(ReleaseCardFrontendDto(m));
            }
            return dtos;
        }

        public List<ReleaseItemFrontendDto> ReleaseItemsFrontendDtos(List<ReleaseItem> models)
        {
            List<ReleaseItemFrontendDto> dtos = new();
            foreach(var m in models)
            {
                dtos.Add(ReleaseItemFrontendDto(m));
            }
            return dtos;
        }

        public ReleaseItemFrontendDto ReleaseItemFrontendDto(ReleaseItem model)
        {
            ReleaseItemFrontendDto dto = new();
            dto.OrderItem = ItemDto(model.OrderItem);
            dto.Quantity = model.Quantity;
            dto.ReleaseId = model.ReleaseId;

            return dto;
        }
        #endregion Release
        #region Virtual
        public List<VirtualOrderCard> VirtualOrdersCards(List<VirtualOrder> models)
        {
            List<VirtualOrderCard> dtos = new();
            foreach(var model in models)
            {
                dtos.Add(VirtualOrderCard(model));
            }

            return dtos;
        }
        public VirtualOrderCard VirtualOrderCard(VirtualOrder model)
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
        public VirtualOrderFrontendDto VirtualOrderFrontendDto(VirtualOrder model)
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

        public ApplicationUser Person(PersonInfo personInfo)
        {
            ApplicationUser person = new ApplicationUser();
            person = _repo.GetPersonById(person.Id);
            return person;
        }


        #endregion FrontendDto --> Original
    }
}
