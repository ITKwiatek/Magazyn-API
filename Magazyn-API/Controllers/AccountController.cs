using Magazyn_API.Data;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order.FrontendDto;
using Magazyn_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserService _userService;
        private readonly IUserRepository _repo;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository repo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repo = repo;
            _userService = new UserService(userManager, roleManager, _repo);
        }
        [HttpGet()]
        public async Task<UserFrontendDto> GetAccountInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);


                var dto = _userService.UserFrontendDto(user);

                return dto;
            }

            return new UserFrontendDto();
        }


        [HttpPut()]
        public async Task<bool> UpdateUserInfo([FromBody] UserFrontendDto dto)
        {
            bool updated = _repo.UpdateUserInfo(dto);

            return updated;
        }


        [HttpPut("password")]
        public async Task<bool> UpdatePassword([FromBody] JObject data)
        {
            string oldPassword = data["oldPassword"].ToString();
            string newPassword = data["newPassword"].ToString();
            if (await isPasswordWeek(newPassword))
                return false;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);


                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (result != null)
                    return true;
            }
            return false;
        }

        private async Task<bool> isPasswordWeek(string password)
        {
            return password.Length < 6 ? true : false;
        }
    }
}
