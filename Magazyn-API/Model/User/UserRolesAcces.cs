using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Model.User
{
    public static class UserRolesAcces
    {
        public static List<string> GetUserRoles()
        {
            var roles = Enum.GetNames(typeof(UserRoles)).ToList();

            return roles;
        }
    }
}
