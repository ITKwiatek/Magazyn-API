using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order.FrontendDto;
using Magazyn_API.Model.User;
using Magazyn_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[EnableCors("MyPolicy")]
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserService _userService;
        private readonly IUserRepository _repo;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository repo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repo = repo;
            _userService = new UserService(userManager, roleManager, _repo);
        }
        [HttpGet()]
        public async Task<List<UserFrontendDto>> GetUsers()
        {
            _userService = new UserService(_userManager, _roleManager, _repo);

            var users = _userService.GetUsersFrontendDto();

            return users;
        }
        [HttpPut()]
        public async Task<bool> UpdateUserInfo([FromBody] UserFrontendDto dto)
        {
            bool updated = _repo.UpdateUserInfo(dto);

            return updated;
        }


        [HttpPut("{id}/roles")]
        public async Task<bool> UpdateUserRoles([FromRoute] string id, [FromBody] List<string> roles)
        {
            var user = _repo.GetUserById(id);
            var allRoles = UserRolesAcces.GetUserRoles();

            foreach (var r in allRoles)
            {
                if (roles.Contains(r))
                    await _userManager.AddToRoleAsync(user, r);
                else
                    await _userManager.RemoveFromRoleAsync(user, r);
            }

            return true;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            try
            {
                var user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
                var deleted = await _userManager.DeleteAsync(user);
                return Ok( new Response() { Message = "Usunięto", Status = "200" });
            } catch (Exception  e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
