using Magazyn_API.Model.Order;
using Magazyn_API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Magazyn_API.Data
{
    public class OrderRepository : IOrderRepository, IDisposable
    {
        private ApplicationDbContext _db;
        private readonly ILogger<ApplicationDbContext> _logger;
        OrderService s;

        public OrderRepository(ApplicationDbContext db, ILogger<ApplicationDbContext> logger)
        {
            _db = db;
            _logger = logger;
            s = new OrderService(this);
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

        public ComponentModel GetComponentById(int id)
        {
            var com = _db.Components.Where(c => c.Id == id).FirstOrDefault();
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

        public Device GetDeviceById(int id)
        {
            var dev = _db.Devices.Where(d => d.Id == id).FirstOrDefault();
            dev.Project = GetProjectById(dev.ProjectId);
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
        public bool ExistsOrderById(int Id)
        {
            if (_db.Orders.Where(o => o.Id == Id).FirstOrDefault() != null)
                return true;
            return false;
        }
        public List<OrderModel> GetAllOrders()
        {
            var orders =_db.Orders.ToList();
            if (orders == null)
                orders = new List<OrderModel>();
            foreach(var o in orders)
            {
                o.Device = GetDeviceById(o.DeviceId);
                o.OrderItems = GetItemsByOrderId(o.Id);
                o.Receiver = new();
                if(o.ReceiverId.HasValue)
                    o.Receiver = GetPersonById((int)o.ReceiverId);
            }
            return orders;
        }
        public OrderModel GetOrderById(int id)
        {
            OrderModel order = _db.Orders.Where(o => o.Id == id).FirstOrDefault();
            order.OrderItems = GetItemsByOrderId(id);
            order.Device = GetDeviceById(order.DeviceId);
            return order;
        }

        public bool UpdateOrder(OrderModel order)
        {
            var orderDb = _db.Orders.FirstOrDefault(o => o.Id == order.Id);
            orderDb.ConfirmedById = order.ConfirmedById;
            orderDb.ReleaseDate = order.ReleaseDate;
            orderDb.DateToRelease = order.DateToRelease;
            orderDb.DateToEP = order.DateToEP;
            orderDb.DateToWarehouse = order.DateToWarehouse;
            orderDb.IssuerId = order.IssuerId;
            orderDb.ReceiverId = order.ReceiverId;
            orderDb.State = s.UpdateState(order.Id);
            foreach(var item in order.OrderItems)
            {
                UpdateItem(item);
            }
            _db.Orders.Update(orderDb);
            _db.SaveChanges();
            
            return true;
        }

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
        public List<OrderItem> GetItemsByOrderId(int orderId)
        {
            List<OrderItem> items = _db.OrderItems.Where(i => i.OrderId == orderId).ToList();
            if (items == null)
                items = new();
            foreach(var item in items)
            {
                item.Component = GetComponentById(item.ComponentId);
            }
            return items;
        }
        public bool UpdateItem(OrderItem item)
        {
            _db.Update(item);
            _db.SaveChanges();
            return true;
        }
        public bool SaveItem(OrderItem item)
        {
            _db.OrderItems.Add(item);
            _db.SaveChanges();
            return true;
        }
        #endregion OrderItem
        #region Project
        public Project GetProjectById(int id)
        {
            var project = _db.Projects.Where(p => p.Id == id).FirstOrDefault();
            if (project == null)
                project = new Project();
            return project;
        }
        public Project GetProjectByName(string name)
        {
            var project = _db.Projects.Where(p => p.Name == name).FirstOrDefault();
            return project;
        }
        #endregion Project
        #region Person
        public Person GetPersonById(int id)
        {
            var p =_db.Persons.Where(p => p.Id == id).FirstOrDefault();
            if (p == null)
                p = new Person();
            return p;
        }
        #endregion Person

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
