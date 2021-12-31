using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
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
        public Device GetDevice(Device device);
        public Device GetDeviceByNameAndProjectName(string name, string projectName);
        public bool SaveDevice(Device device);
        #endregion Device  

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
        public bool UpdateItem(OrderItem item);
        public bool SaveItem(OrderItem item);
        #endregion Item
        public List<OrderItem> GetItemsByOrderId(int orderId);

        #region Project
        public Project GetProjectByName(string name);
        #endregion Project
        #region Person
        public Person GetPersonById(int id);
        #endregion Person
        #region VirtualOrder
        public List<VirtualOrderModel> GetAllVirtualOrdersWithItems();
        public VirtualOrderModel GetVirtualOrderById(int id);
        #endregion VirtualOrder
        #region VirtualItems
        public VirtualItem GetVirtualItemById(int id);
        #endregion VirtualItems
    }


}