using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public List<Device> Devices { get; set; }
    }
}
