using AutoMapper.Configuration;
using Magazyn_API.Data;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }
        #region Register/Login
        [HttpPost("register")]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            string email = model.Email.ToUpperInvariant();
            var userExists = await _userManager.FindByEmailAsync(model.Email.ToUpperInvariant());
            if (userExists != null)
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = "Email jest zajęty"
                });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email.ToUpperInvariant(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email.ToLowerInvariant(),
                FirstName = model.FirstName,
                Surname = model.Surname
                //UserName = "UserName"
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string message = result.Errors.ElementAt(0).Description.ToString();
                return BadRequest( new Response { Status = "Error", Message = message });
            }
                

            _db.SaveChanges();

            return Ok(new Response
            {
                Status = "Succes",
                Message = "Rejestracja udana. Teraz możesz się zalogować"
            });
        }
        [HttpPost("isEmailAvailable")]
        [EnableCors("MyPolicy")]
        public async Task<bool> IsEmailAvailable([FromBody] JObject data)
        {
            string email = data["email"].ToString();
            if (_db.Users.Any(u => u.NormalizedEmail == email.ToUpperInvariant()))
                return false;

            return true;
        }
        #endregion Register/Login
        #region Token
        [HttpGet()]
        [Authorize]
        public async Task<bool> IsTokenValid()
        {
            return true;
        }
        [HttpPost("token")]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> Create([FromBody] LoginModel model)
        {
            if (await IsValidEmailAndPassword(model.Email, model.Password))
            {
                return new ObjectResult(await GenerateToken(model.Email));
            }
            else
            {
                return Unauthorized(new Response() { Message = "Złe dane logowania", Status = "401" });
            }
        }
        private async Task<bool> IsValidEmailAndPassword(string email, string passoword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passoword))
                return false;
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.CheckPasswordAsync(user, passoword);
        }
        private async Task<dynamic> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = from ur in _db.UserRoles
                        join r in _db.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                                            new JwtHeader(
                                                            new SigningCredentials(
                                                                                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!@#Rasfaeveaia@#!122fa351wfawfaWFWAGhbehaGTWT")),
                                                                                    SecurityAlgorithms.HmacSha256)),
                                            new JwtPayload(claims));

            var output = new
            {
                Acces_Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = email
            };

            return output;
        }
        #region Logout
        //[HttpGet("/deleteTokens")]
        //public async Task<bool> DeleteTokensByIp()
        //{
        //    string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //    MongoOwnClient mongo = MongoOwnClient.GetInstance();
        //    await mongo.DeleteTokensWithThisIpAsync(remoteIpAddress);
        //    return true;
        //}
        #endregion Logout
        #endregion Token
    }
}
