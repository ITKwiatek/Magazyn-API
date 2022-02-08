using Magazyn_API.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Shortage
{
    public class ShortageItem
    {
        public int Count { get; set; }
        public string ProjectName { get; set; }
        public string GroupName { get; set; }
        public string DeviceName { get; set; }
        public ComponentModel Component { get; set; }
    }
}
