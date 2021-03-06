using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Magazyn_API.Model.Shortage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magazyn_API.Data
{
    public interface IOrderRepository
    {
        #region Component
        public ComponentModel GetComponent(IComponentModel component);
        public bool ExistsInDb(IComponentModel component);
        public bool SaveComponent(ComponentModel model);
        #endregion Component

        #region Device
        public bool ExistsInDb(Device device);
        public List<string> GetAllDeviceNames();
        public Device GetDevice(Device device);
        public Device GetDeviceByNameGroupNameAndProjectName(string name, string groupName, string projectName);
        public Device GetOrCreateDeviceByNameGroupNameAndProjectName(string name, string groupName, string projectName);
        public bool SaveDevice(Device device);
        #endregion Device  
        #region Group
        public List<string> GetAllGroupNames();
        public GroupModel GetGroupByNameAndProjectName(string name, string projectName);
        #endregion Group
        #region Order
        public bool DeleteOrderById(int id);
        public List<OrderModel> GetAllOrders();
        public List<OrderModel> GetActiveOrders();
        public List<OrderModel> GetInActiveOrders();
        public List<OrderModel> GetNewOrders();
        public List<OrderModel> GetUnfinishedOrders();
        public List<OrderModel> GetFinishedOrders();
        public int GetOrderId(OrderModel order);
        public OrderModel GetOrderWithItemsById(int id);
        public OrderModel GetOrderWithoutItemsById(int id);
        public bool UpdateOrderWithItems(OrderModel order);
        public bool UpdateOrderDetails(OrderModel order);
        public bool UpdateOrderDetails(OrderModelFrontendDto order);
        public bool SaveOrder(OrderModel order);
        #endregion Order

        #region Item
        public OrderItem GetItemById(int id);
        public List<OrderItem> GetItemsByOrderId(int orderId);
        public bool UpdateItem(OrderItem item);
        public bool SaveItem(OrderItem item);
        #endregion Item

        #region Project
        public List<string> GetAllProjectNames();
        public Project GetProjectByName(string name);
        #endregion Project
        #region Person
        public ApplicationUser GetPersonById(string id);
        #endregion Person
        #region Release
        public Release GetReleaseWithoutItemsById(int id);
        public Release GetReleaseWithItemsById(int id);
        public List<Release> GetAllReleasesWithItems();
        #endregion Release
        #region ReleaseItem
        public List<ReleaseItem> GetReleaseItemsByReleaseId(int releaseId);
        #endregion ReleaseItem
        #region Shortages
        public List<ShortageItem> GetComponentItemsByProjectGroupAndDevice(string projectName, string groupName, string deviceName);
        public List<ShortageItem> GetComponentItemsByOrderId(int orderId);
        #endregion Shortages
        #region VirtualOrder
        public bool DeleteVirtualOrderById(int id);
        public List<VirtualOrder> GetAllVirtualOrdersWithItems();
        public VirtualOrder GetVirtualOrderWithItemsById(int id);
        public VirtualOrder GetVirtualOrderById(int id);
        #endregion VirtualOrder
        #region VirtualItems
        public VirtualItem GetVirtualItemById(int id);
        #endregion VirtualItems
    }


}