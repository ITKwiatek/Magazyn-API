using Magazyn_API.Model.Order;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Magazyn_API.Data
{
    public class OrderRepository : IOrderRepository, IDisposable
    {
        private ApplicationDbContext _db;
        private readonly ILogger<ApplicationDbContext> _logger;

        public OrderRepository(ApplicationDbContext db, ILogger<ApplicationDbContext> logger)
        {
            _db = db;
            _logger = logger;
        }
        #region Component

        public ComponentModel GetComponent(IComponentModel component)
        {
            var com = _db.Components
                .Where(c => c.ArticleNumber == component.ArticleNumber)
                .Where(c => c.Description == component.Description)
                .Where(c => c.OrderingNumber == component.OrderingNumber)
                .Where(c => c.SAP == component.SAP)
                .Where(c => c.Supplier == component.Supplier)
                .FirstOrDefault();
            return com;
        }

        public bool ExistsInDb (IComponentModel component)
        {
            var com = GetComponent(component);
            return com != null;
        }

        public bool SaveComponent(ComponentModel model)
        {
            var response = false;
            if (!ExistsInDb(model))
            {
                _db.Components.Add(model);
                _db.SaveChanges();
                response = true;
            }
            return response;
        }
        #endregion Component
        #region Device
        public bool ExistsInDb(Device device)
        {
            var dev = GetDeviceByNameAndProjectName(device.Name, device.Project.Name);
            return dev != null;
        }

        public Device GetDevice(Device device)
        {
            var proj = GetProjectByName(device.Project.Name);
            var dev = _db.Devices
                .Where(d => d.Name == device.Name)
                .Where(d => d.Project == proj)
                .FirstOrDefault();
            return dev;
        }

        public Device GetDeviceByNameAndProjectName(string name, string projectName)
        {
            var proj = GetProjectByName(projectName);
            var dev = _db.Devices
                .Where(d => d.Name == name)
                .Where(d => d.Project == proj)
                .FirstOrDefault();
            return dev;
        }

        public bool SaveDevice(Device device)
        {
            if (!ExistsInDb(device))
            {
                _db.Devices.Add(device);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion Device
        #region Order

        public bool SaveOrder(OrderModel order)
        {
            if (ExistsInDb(order.Device))
                order.Device = GetDevice(order.Device);
            foreach (var item in order.OrderItems)
            {
                if (ExistsInDb(item.Component))
                    item.Component = GetComponent(item.Component);
            }

            _db.Orders.Add(order);
            _db.SaveChanges();
            return true;
        }
        #endregion Order
        #region OrderItem
        public bool SaveItem(OrderItem item)
        {
            _db.OrderItems.Add(item);
            _db.SaveChanges();
            return true;
        }
        #endregion OrderItem
        #region Project
        public Project GetProjectByName(string name)
        {
            var project = _db.Projects.Where(p => p.Name == name).FirstOrDefault();
            return project;
        }
        #endregion Project

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
