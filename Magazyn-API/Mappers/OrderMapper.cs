using Magazyn_API.Data;
using Magazyn_API.Migrations;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FromExcelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Mappers
{
    public class OrderMapper
    {
        public OrderMapper(IOrderRepository repo)
        {
            _repo = repo;
        }

        private readonly IOrderRepository _repo;
        public OrderModel Order(OrderModelFromExcelDto orderDto)
        {
            OrderModel order = new();
            order.Device = Device(orderDto.DeviceName, orderDto.GroupName, orderDto.ProjectName);
            foreach(var itemDto in orderDto.OrderItems)
            {
                OrderItem item = new();
                item.Component = Component(itemDto.Component);
                item.Order = order;
                item.RequiredQuantity = itemDto.Quantity;
                order.OrderItems.Add(item);
            }
            return order;
        }

        public ComponentModel Component(ComponentModelFromExcelDto componentDto)
        {
            ComponentModel comp = new();
            if (!_repo.ExistsInDb(componentDto))
            {
                comp.ArticleNumber = componentDto.ArticleNumber;
                comp.Description = componentDto.Description;
                comp.SAP = componentDto.SAP;
                comp.Supplier = componentDto.Supplier;
                comp.OrderingNumber = componentDto.OrderingNumber;
                return comp;
            }
            else
                return _repo.GetComponent(componentDto);
        }

        public Device Device(string deviceName, string groupName, string projectName)
        {
            var dev = _repo.GetOrCreateDeviceByNameGroupNameAndProjectName(deviceName, groupName, projectName);
            if (dev == null)
                dev = new(deviceName);
            dev.Group = GroupModel(groupName, projectName);
            return dev;
        }

        public GroupModel GroupModel(string groupName, string projectName)
        {
            var group = _repo.GetGroupByNameAndProjectName(groupName, projectName);
            if (group == null)
                group = new GroupModel() { Name = groupName, Project = Project(projectName) };

            return group;
        }

        public Project Project(string projectName)
        {
            var project = _repo.GetProjectByName(projectName);
            if (project == null)
                project = new(projectName);
            return project;
        }
    }
}
