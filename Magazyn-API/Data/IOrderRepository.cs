using Magazyn_API.Model.Order;

namespace Magazyn_API.Data
{
    public interface IOrderRepository
    {
        public ComponentModel GetComponent(IComponentModel component);

        public bool ExistsInDb(IComponentModel component);

        public bool SaveComponent(ComponentModel model);
        public bool ExistsInDb(Device device);

        public Device GetDevice(Device device);

        public Device GetDeviceByNameAndProjectName(string name, string projectName);
        public bool SaveDevice(Device device);


        public bool SaveOrder(OrderModel order);

        public bool SaveItem(OrderItem item);
        public Project GetProjectByName(string name);
    }
}