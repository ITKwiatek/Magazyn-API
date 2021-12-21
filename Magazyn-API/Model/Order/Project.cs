using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class Project
    {
        public Project(string name)
        {
            Name = name;
        }
        public Project()
        {

        }
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int Number { get; set; }
        public List<Device> Devices { get; set; }
    }
}
