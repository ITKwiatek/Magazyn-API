using Magazyn_API.Model.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Data
{
    public interface IUserRepository
    {
        public ApplicationUser GetUserById(string id);
        public List<ApplicationUser> GetAllUsers();
        public List<IdentityRole> GetUserRolesByUserId(string id);
        public List<IdentityRole> GetAllRoles();
        public IdentityRole GetRoleByName(string name);
    }
}
