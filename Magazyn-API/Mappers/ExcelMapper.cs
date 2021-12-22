using AutoMapper;
using Magazyn_API.Data;
using Magazyn_API.Model.Excel;
using Magazyn_API.Model.Mappers;
using Magazyn_API.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.AutoMapper
{
    public class ExcelMapper
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ExcelMapper(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }
        public OrderModel MapToOrder(IExcelOrder eOrder)
        {
            OrderModel order = new();

            var project = _db.Projects.Where(p => p.Name == eOrder.ProjectName).FirstOrDefault();
            if(project == null)
            {
                project = new Project(eOrder.ProjectName);
            }

            var device = _db.Devices.Where(d => d.Name == eOrder.DeviceName).FirstOrDefault();
            if (device == null)
            {
                device = new Device(eOrder.DeviceName);
                device.Project = project;
            }
            order.Device = device;

            var items = new List<OrderItem>();
            foreach(var eI in eOrder.OrderItems)
            {
                var item = new OrderItem();
                item.RequiredQuantity = eI.Count;
                item.Order = order;
                item.Component = _mapper.Map<ComponentModel>(eI.ExcelComponent);
                order.OrderItems.Add(item);
            }

            if (eOrder.DateToEP != DateTime.MinValue)
                order.DateToEP = eOrder.DateToEP;
            if (eOrder.DateToWarehouse != DateTime.MinValue)
                order.DateToWarehouse = eOrder.DateToWarehouse;
            if (eOrder.ReleaseDate != DateTime.MinValue)
                order.DateRelease = eOrder.ReleaseDate;

            return order;
        }
    }
}
