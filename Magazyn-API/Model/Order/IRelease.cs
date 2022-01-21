using Magazyn_API.Model.Order.FrontendDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order
{
    public interface IRelease
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual IPerson Issuer 
        {
            get { return Issuer; }
            set { Issuer = value; }
        }
        public DateTime ReleasedDate { get; set; }
        public string ReceiverInfo { get; set; }
    }
}
