using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class Device
    {
        public Device()
        {

        }
        public Device(string name)
        {
            Name = name;
        }
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public List<OrderModel> Orders { get; set; }
    }
}
