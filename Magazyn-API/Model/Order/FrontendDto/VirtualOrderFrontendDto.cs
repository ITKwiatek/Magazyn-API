using Magazyn_API.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class VirtualOrderFrontendDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<int> OrdersIds { get; set; }
        public IEnumerable<VirtualItemFrontendDto> Items { get; set; }
    }
}
