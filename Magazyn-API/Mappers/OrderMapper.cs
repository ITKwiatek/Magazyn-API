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
            order.Device = Device(orderDto.DeviceName, orderDto.ProjectName);
            foreach(var itemDto in orderDto.OrderItems)
            {
                OrderItem item = new();
                item.Component = Component(itemDto.Component);
                item.Order = order;
                item.Quantity = itemDto.Quantity;
                order.OrderItems.Add(item);
            }
            return order;
        }

        public ComponentModel Component(ComponentModelFromExcelDto componentDto)
        {
            if (!_repo.ExistsInDb(componentDto))
                return new ComponentModel(
                    componentDto.ArticleNumber,
                    componentDto.Description,
                    componentDto.OrderingNumber,
                    componentDto.SAP,
                    componentDto.Supplier);
            else
                return _repo.GetComponent(componentDto);
        }

        public Device Device(string deviceName, string projectName)
        {
            var dev = _repo.GetDeviceByNameAndProjectName(deviceName, projectName);
            if (dev == null)
                dev = new(deviceName);
            dev.Project = Project(projectName);
            return dev;
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
