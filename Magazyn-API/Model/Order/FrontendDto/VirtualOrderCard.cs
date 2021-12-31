using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class VirtualOrderCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PersonInfo? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<int> OrdersIds { get; set; } = new();
    }
}
