using Magazyn_API.Model.Order;
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
        public List<OrderModel> GetAllOrders();
        public OrderModel GetOrderById(int id);
        public bool UpdateOrder(OrderModel order);
        public bool SaveOrder(OrderModel order);
        #endregion Order

        #region Item
        public bool SaveItem(OrderItem item);
        #endregion Item
        public List<OrderItem> GetItemsByOrderId(int orderId);

        #region Project
        public Project GetProjectByName(string name);
        #endregion Project
    }
}