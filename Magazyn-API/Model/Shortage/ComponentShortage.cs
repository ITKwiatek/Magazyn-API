using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Shortage
{
    public class ComponentShortage
    {
        public int ComponentId { get; set; }
        public int Count { get; set; }
        public List<ShortageItem> Items { get; set; }
    }
}
