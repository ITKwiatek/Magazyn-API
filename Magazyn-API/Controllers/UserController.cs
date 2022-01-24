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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            try
            {
                var user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
                var deleted = await _userManager.DeleteAsync(user);
                return Ok(deleted);
            } catch (Exception  e)
            {
                return BadRequest(e.Message);
            }
        }
        #region Role
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignNewRoleToUser([FromBody] JObject data)
        {
            try
            {
                string userId = data["id"].ToString();
                string userRole = data["role"].ToString();

                var user = _repo.GetUserById(userId);
                var role = _repo.GetRoleByName(userRole);
                await _userManager.AddToRoleAsync(user, role.Name);

                var dto = _userService.UserFrontendDto(user);

                return Ok(dto);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("RemoveFromRole")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] JObject data)
        {
            try
            {
                string userId = data["id"].ToString();
                string userRole = data["role"].ToString();

                var user = _repo.GetUserById(userId);
                var role = _repo.GetRoleByName(userRole);

                await _userManager.RemoveFromRoleAsync(user, role.Name);
                var dto = _userService.UserFrontendDto(user);

                return Ok(dto);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion Role
    }
}
