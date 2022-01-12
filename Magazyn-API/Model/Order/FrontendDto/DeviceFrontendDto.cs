using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class DeviceFrontendDto
    {
        public int Id { get; set; }
        public ProjectFrontendDto Project { get; set; }
        public string Name { get; set; }
    }
}
