using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.Order.FrontendDto
{
    public class UserFrontendDto : IPerson
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get ; set; }
        public string Email { get; set; }
        public List<string> Roles {get; set;} = new List<string>();
    }
}
