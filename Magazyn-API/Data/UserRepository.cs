using Magazyn_API.Model.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Data
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<IdentityRole> GetAllRoles()
        {
            var roles = _db.Roles.ToList();

            return roles;
        }

        public IdentityRole GetRoleByName(string name)
        {
            var role = _db.Roles.Where(r => r.Name.ToUpper() == name.ToUpper()).FirstOrDefault();

            return role;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            var users = _db.Users.ToList();
            return users;
        }

        public ApplicationUser GetUserById(string id)
        {
            var user = _db.Users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }

        public List<IdentityRole> GetUserRolesByUserId(string id)
        {
            var roleIds = _db.UserRoles.Where(e => e.UserId == id).ToList();
            var roles = new List<IdentityRole>();
            foreach(var rId in roleIds)
            {
                roles.Add(_db.Roles.Where(r => r.Id == rId.RoleId).FirstOrDefault());
            }
            return roles;
        }
    }
}
