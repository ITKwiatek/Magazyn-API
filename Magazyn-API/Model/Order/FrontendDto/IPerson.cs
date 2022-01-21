using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public interface IPerson
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
