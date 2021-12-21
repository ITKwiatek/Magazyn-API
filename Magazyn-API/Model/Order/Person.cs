using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Surname { get; set; }
        public List<Release> ReleaseReceivers { get; set; }
        public List<OrderModel> OrderIssuers { get; set; }
        public List<OrderModel> OrderConfirmings { get; set; }

        //public Release Release { get; set; }
        //public Order Order { get; set; }
    }
}
