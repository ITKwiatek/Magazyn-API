using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Magazyn_API.Model.Shortage;
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
            var dev = GetDeviceByNameGroupNameAndProjectName(device.Name, device.Group.Name, device.Group.Project.Name);
            return dev != null;
        }

        public Device CreateDevice(string name, GroupModel group)
        {
            var device = GetDeviceByNameGroupNameAndProjectName(name, group.Name, group.Project.Name);
            if(device == null)
            {
                device = new Device() { Name = name, Group = group };
                _db.SaveChanges();
            }
            return device;
        }

        public List<string> GetAllDeviceNames()
        {
            List<string> devices = _db.Devices
                .Where(p => p.Name != "")
                .Where(p => p.Name != null)
                .Select(p => p.Name)
                .Distinct()
                .ToList();


            return devices;
        }

        public Device GetDevice(Device device)
        {
            var proj = GetProjectByName(device.Group.Project.Name);
            var dev = _db.Devices
                .Where(d => d.Name == device.Name)
                .Where(d => d.Group.Name == device.Group.Name)
                .Where(d => d.Group.Project == proj)
                .FirstOrDefault();
            return dev;
        }

        public Device GetDeviceById(int id)
        {
            var dev = _db.Devices.Where(d => d.Id == id).FirstOrDefault();
            dev.Group = GetGroupById(dev.GroupId);
            return dev;
        }

        public Device GetDeviceByNameGroupNameAndProjectName(string name, string groupName, string projectName)
        {
            var dev = _db.Devices
                .Where(d => d.Name == name)
                .Where(d => d.Group.Name == groupName)
                .Where(d => d.Group.Project.Name == projectName)
                .FirstOrDefault();

            return dev;
        }

        public Device GetOrCreateDeviceByNameGroupNameAndProjectName(string name, string groupName, string projectName)
        {
            var proj = GetProjectByName(projectName);
            if (proj == null)
                proj = CreateProject(projectName);
            var gr = GetGroupByNameAndProjectName(groupName, projectName);
            if (gr == null)
                gr = CreateGroup(groupName, proj);
            var dev = GetDeviceByNameGroupNameAndProjectName(name, groupName, projectName);
            if (dev == null)
                dev = CreateDevice(name, gr);

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
        #region Group
        public GroupModel CreateGroup(string name, Project project)
        {
            var group = GetGroupByNameAndProjectName(name, project.Name);
            if (group == null)
            {
                group = new GroupModel() { Name = name, Project = project };
                _db.SaveChanges();
            }
                
            return group;
        }
        public List<string> GetAllGroupNames()
        {
            List<string> groups = _db.Groups       
                .Where(p => p.Name != "")
                .Where(p => p.Name != null)
                .Select(p => p.Name)
                .Distinct()
                .ToList();
            return groups;
        }
        public GroupModel GetGroupById(int id)
        {
            var gr = _db.Groups.Where(d => d.Id == id).FirstOrDefault();
            gr.Project = GetProjectById(gr.ProjectId);
            return gr;
        }
        public GroupModel GetGroupByNameAndProjectName(string name, string projectName)
        {
            var ret = _db.Groups
                .Where(g => g.Name == name)
                .Where(g => g.Project.Name == projectName)
                .FirstOrDefault();

            return ret;
        }
        #endregion Group
        #region Order
        public bool ExistsOrderById(int Id)
        {
            if (_db.Orders.Where(o => o.Id == Id).FirstOrDefault() != null)
                return true;
            return false;
        }
        public bool DeleteOrderById(int id)
        {
            var order = GetOrderWithItemsById(id);
            //foreach(var item in order.OrderItems)
            //{
            //    DeleteItem(item);
            //}
            if (order == null)
                return false;
            _db.Orders.Remove(order);
            _db.SaveChanges();
            return true;
        }
        public List<OrderModel> GetOrdersByVirtualOrderId(int virtualOrderId)
        {
            List<OrderModel> orders = new();
            List<VirtualManyToMany> many = _db.ManyToMany.Where(m => m.VirtualOrderId == virtualOrderId).ToList();
            foreach (var m in many)
            {
                var order = GetOrderWithItemsById(m.OrderId);
                if(!orders.Contains(order))
                    orders.Add(order);
            }

            return orders;
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
            }
            return orders;
        }
        public List<OrderModel> GetActiveOrders()
        {
            var orders = _db.Orders.Where(o => o.IsActive).ToList();
            if (orders == null)
                orders = new List<OrderModel>();
            foreach (var o in orders)
            {
                o.Device = GetDeviceById(o.DeviceId);
                o.OrderItems = GetItemsByOrderId(o.Id);
            }
            return orders;
        }
        public List<OrderModel> GetInActiveOrders()
        {
            var orders = _db.Orders.Where(o => o.IsActive == false).ToList();
            if (orders == null)
                orders = new List<OrderModel>();
            foreach (var o in orders)
            {
                o.Device = GetDeviceById(o.DeviceId);
                o.OrderItems = GetItemsByOrderId(o.Id);
            }
            return orders;
        }
        public List<OrderModel> GetNewOrders()
        {
            var orders = _db.Orders
                .Where(o => o.State == OrderState.NOWY)
                .Where(o => o.IsActive)
                .ToList();
            if (orders == null)
                orders = new List<OrderModel>();
            foreach (var o in orders)
            {
                o.Device = GetDeviceById(o.DeviceId);
                o.OrderItems = GetItemsByOrderId(o.Id);
            }
            return orders;
        }
        public List<OrderModel> GetUnfinishedOrders()
        {
            var orders = _db.Orders
                .Where(o => o.State == OrderState.W_TRAKCIE)
                .Where(o => o.IsActive)
                .ToList();
            if (orders == null)
                orders = new List<OrderModel>();
            foreach (var o in orders)
            {
                o.Device = GetDeviceById(o.DeviceId);
                o.OrderItems = GetItemsByOrderId(o.Id);
            }
            return orders;
        }
        public List<OrderModel> GetFinishedOrders()
        {
            var orders = _db.Orders
                .Where(o => o.State == OrderState.ZAKONCZONY)
                .Where(o => o.IsActive)
                .ToList();
            if (orders == null)
                orders = new List<OrderModel>();
            foreach (var o in orders)
            {
                o.Device = GetDeviceById(o.DeviceId);
                o.OrderItems = GetItemsByOrderId(o.Id);
            }
            return orders;
        }
        public OrderModel GetOrderWithoutItemsById(int id)
        {
            OrderModel order = _db.Orders.Where(o => o.Id == id).FirstOrDefault();
            if (order == null)
                return new OrderModel();
            order.Device = GetDeviceById(order.DeviceId);
            return order;
        }
        public OrderModel GetOrderWithItemsById(int id)
        {
            OrderModel order = _db.Orders.Where(o => o.Id == id).FirstOrDefault();
            if (order == null)
                return order;
            order.OrderItems = GetItemsByOrderId(id);
            order.Device = GetDeviceById(order.DeviceId);
            return order;
        }

        public int GetOrderId(OrderModel order)
        {
            _db.Entry(order).GetDatabaseValues();
            return order.Id;
        }

        public bool UpdateOrderDetails(OrderModel order)
        {
            if (order.Id == 0)
                return false;
            OrderModel orderDb = GetOrderWithItemsById(order.Id);
            orderDb.IsActive = order.IsActive;
            if(order.ReleaseDate > DateTime.MinValue)
                orderDb.ReleaseDate = order.ReleaseDate;
            if (order.ConfirmedBy != null)
                orderDb.ConfirmedBy = order.ConfirmedBy;
            if (order.DateToRelease > DateTime.MinValue)
                orderDb.DateToRelease = order.DateToRelease;
            if (order.DateToWarehouse > DateTime.MinValue)
                orderDb.DateToWarehouse = order.DateToWarehouse;

            _db.Orders.Update(orderDb);
            _db.SaveChanges();

            return true;
        }

        public bool UpdateOrderDetails(OrderModelFrontendDto orderDto)
        {
            if (orderDto.Id == 0)
                return false;
            OrderModel orderDb = GetOrderWithItemsById(orderDto.Id);
            if (orderDto.DateToRelease > DateTime.MinValue)
                orderDb.DateToRelease = orderDto.DateToRelease;
            if (!orderDb.IsActive && orderDto.IsActive)
                orderDb.DateToWarehouse = DateTime.Now;

            if (!orderDb.IsActive && orderDto.IsActive)
                orderDto.DateToWarehouse = DateTime.Now;
            orderDb.IsActive = orderDto.IsActive;

            if(orderDb.Device.Name != orderDto.Device.Name || orderDb.Device.Group.Name != orderDto.Device.Group.Name || orderDb.Device.Group.Project.Name != orderDto.Device.Group.Project.Name)
            {
                orderDb.Device = GetOrCreateDeviceByNameGroupNameAndProjectName(orderDto.Device.Name, orderDto.Device.Group.Name, orderDto.Device.Group.Project.Name);
            }

            _db.Orders.Update(orderDb);
            _db.SaveChanges();

            return true;
        }

        public bool UpdateOrderWithItems(OrderModel order)
        {
            var orderDb = _db.Orders.FirstOrDefault(o => o.Id == order.Id);
            orderDb.ConfirmedById = order.ConfirmedById;
            orderDb.ReleaseDate = order.ReleaseDate;
            orderDb.DateToRelease = order.DateToRelease;
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
        public bool ExistsInDb(OrderItem item)
        {
            if (GetItemById(item.Id) != null)
                return true;
            return false;
        }
        public bool DeleteItem(OrderItem item)
        {
            _db.OrderItems.Remove(item);
            _db.SaveChanges();
            return true;
        }
        public OrderItem GetItemById(int id)
        {
            var item = _db.OrderItems.FirstOrDefault(i => i.Id == id);
            item.Component = GetComponentById(item.ComponentId);
            return item;
        }
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
            if (ExistsInDb(item))
            {
                _db.Update(item);
                _db.SaveChanges();
                ChangeOrderState(item.OrderId);
                return true;
            }
            return false;
        }
        public bool SaveItem(OrderItem item)
        {
            _db.OrderItems.Add(item);
            _db.SaveChanges();
            return true;
        }
        #endregion OrderItem
        #region Project
        public Project CreateProject(string name)
        {
            var project = GetProjectByName(name);
            if (project == null)
            {
                project = new() { Name = name };
                _db.SaveChanges();
            }
            return project;
        }
        public List<string> GetAllProjectNames()
        {
            List<string> projects = _db.Projects
                .Where(p => p.Name != "")
                .Where(p => p.Name != null)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            return projects;
        }
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
        public ApplicationUser GetPersonById(string id)
        {
            var p =_db.Users.Where(p => p.Id == id).FirstOrDefault();

            return p;
        }
        #endregion Person
        #region Release
        public Release GetReleaseWithoutItemsById(int id)
        {
            var model = _db.Releases.FirstOrDefault(r => r.Id == id);
            if(model != null)
            {
                model.Issuer = GetPersonById(model.IssuerId);
                model.Receiver = GetPersonById(model.ReceiverId);
            }
            return model;
        }
        public Release GetReleaseWithItemsById(int id)
        {
            var model = GetReleaseWithoutItemsById(id);

            if(model != null)
                model.ReleaseItems = GetReleaseItemsByReleaseId(id);

            return model;
        }
        public List<Release> GetAllReleasesWithItems()
        {
            var models = _db.Releases.ToList();

            foreach(var m in models)
            {
                m.ReleaseItems = GetReleaseItemsByReleaseId(m.Id);
                m.Issuer = GetPersonById(m.IssuerId);
                m.Order = GetOrderWithoutItemsById(m.OrderId);
            }

            return models;
        }
        #endregion Release
        #region ReleaseItem
        public ReleaseItem GetReleaseItem(int releaseId, int orderItemId)
        {
            var model = _db.ReleaseItems
                .Where(i => i.ReleaseId == releaseId)
                .Where(i => i.OrderItemId == orderItemId)
                .FirstOrDefault();

            return model;
        }
        public List<ReleaseItem> GetReleaseItemsByReleaseId(int releaseId)
        {
            var models = _db.ReleaseItems
                .Where(i => i.ReleaseId == releaseId)
                .ToList();

            return models;
        }
        #endregion ReleaseItem
        #region Shortages
        public List<ShortageItem> GetComponentItemsByOrderId(int orderId)
        {
            List<ShortageItem> items = new();
            items = _db.OrderItems
                .Where(c => c.Order.State == OrderState.W_TRAKCIE)
                .Where(c => c.RequiredQuantity > c.CurrentQuantity)
                .Where(c => c.OrderId == orderId)
                .Select(c => new ShortageItem()
                {
                    Count = c.RequiredQuantity - c.CurrentQuantity,
                    ProjectName = c.Order.Device.Group.Project.Name,
                    GroupName = c.Order.Device.Group.Name,
                    DeviceName = c.Order.Device.Name,
                    Component = c.Component

                }).ToList();

            return items;
        }
        public List<ShortageItem> GetComponentItemsByProjectGroupAndDevice(string projectName, string groupName, string deviceName)
        {
            List<ShortageItem> items = new();
            items = _db.OrderItems
                .Where(c => c.Order.State == OrderState.W_TRAKCIE)
                .Where(c => c.RequiredQuantity > c.CurrentQuantity)
                .Where(c => (deviceName != "") ? c.Order.Device.Name == deviceName : c.Order.Device.Name.Contains(""))
                .Where(c => (groupName != "") ? c.Order.Device.Group.Name == groupName : c.Order.Device.Group.Name.Contains(""))
                .Where(c => (projectName != "") ? c.Order.Device.Group.Project.Name == projectName : c.Order.Device.Group.Project.Name.Contains(""))
                .Select(c => new ShortageItem()
                {
                    Count = c.RequiredQuantity - c.CurrentQuantity,
                    ProjectName = c.Order.Device.Group.Project.Name,
                    GroupName = c.Order.Device.Group.Name,
                    DeviceName = c.Order.Device.Name,
                    Component = c.Component

                }).ToList();

            return items;
        }
        #endregion Shortages
        #region VirtualOrder
        public bool DeleteVirtualOrderById(int id)
        {
            VirtualOrder virtualOrderModel = GetVirtualOrderById(id);
            if (virtualOrderModel == null)
                return false;
            _db.VirtualOrders.Remove(virtualOrderModel);
            _db.SaveChanges();
            return true;
        }
        public List<VirtualOrder> GetAllVirtualOrdersWithItems()
        {
            List < VirtualOrder > vOrders = _db.VirtualOrders.ToList();
            List<int> ids = new();

            if (vOrders == null)
                return new List<VirtualOrder>();
            foreach(var vOrder in vOrders)
            {
                List<VirtualManyToMany> many = _db.ManyToMany.Where(m => m.VirtualOrderId == vOrder.Id).ToList();
                foreach (var m in many)
                {
                    var order = GetOrderWithItemsById(m.OrderId);
                    //vOrder.Orders.Add(order);
                }
                vOrder.OrderItems = GetVirtualItemsByVitrualOrderId(vOrder.Id);
            }

            return vOrders;
        }
        public VirtualOrder GetVirtualOrderById(int id)
        {
            VirtualOrder model = _db.VirtualOrders.FirstOrDefault(v => v.Id == id);

            return model;
        }
        public VirtualOrder GetVirtualOrderWithItemsById(int id)
        {
            VirtualOrder model = _db.VirtualOrders.FirstOrDefault(v => v.Id == id);
            if (model == null)
                model = new VirtualOrder();

            model.OrderItems = GetVirtualItemsByVitrualOrderId(id);
            model.Orders = GetOrdersByVirtualOrderId(id);
            return model;
        }
        #endregion VirtualOrder
        #region VirtualItems
        public VirtualItem GetVirtualItemById(int id)
        {
            VirtualItem model = _db.VirtualItems.FirstOrDefault(v => v.Id == id);
            if (model == null)
                model = new VirtualItem();
            return model;
        }
        public List<VirtualItem> GetVirtualItemsByVitrualOrderId(int virtualOrderId)
        {
            List<VirtualItem> vItems = _db.VirtualItems.Where(v => v.VirtualOrderId == virtualOrderId).ToList();
            if (vItems == null)
                vItems = new List<VirtualItem>();
            return vItems;
        }
        #endregion VirtualItems

        #region Utility
        private void ChangeOrderState(int OrderId)
        {
            OrderModel order = GetOrderWithItemsById(OrderId);
            OrderState state = order.State;
            if (state == OrderState.ZAKONCZONY)
                return;
            int itemsFinished = 0;
            foreach(var item in order.OrderItems)
            {
                if(item.CurrentQuantity == item.RequiredQuantity)
                {
                    itemsFinished++;
                    continue;
                } else
                {
                    if(itemsFinished > 1)
                    {
                        order.State = OrderState.W_TRAKCIE;
                        break;
                    }
                }
            }
            if (itemsFinished == order.OrderItems.Count && itemsFinished > 0)
                order.State = OrderState.ZAKONCZONY;
            _db.Orders.Update(order);
            _db.SaveChanges();
        }
        #endregion Utility

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
