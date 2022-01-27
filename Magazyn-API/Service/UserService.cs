using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order.FrontendDto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Service
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _repo;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository repo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repo = repo;
        }
        public List<UserFrontendDto> GetUsersFrontendDto()
        {
            var users = _repo.GetAllUsers();
            var dtos = AssignRolesToUsers(users);
            return dtos;
        }


        public List<string> AssignRolesToUser(ApplicationUser user)
        {
            var userRoles = _repo.GetUserRolesByUserId(user.Id);
            var rolesDto = new List<string>();
            foreach(var role in userRoles)
            {
                rolesDto.Add(role.Name);
            }
            return rolesDto;
        }

        private List<UserFrontendDto> AssignRolesToUsers(List<ApplicationUser> users)
        {
            List<UserFrontendDto> dtos = new();
            foreach (var user in users)
            {
                var dto = UserFrontendDto(user);
                dtos.Add(dto);
            }
            return dtos;
        }
        #region Mapper
        public UserFrontendDto UserFrontendDto(ApplicationUser model)
        {
            UserFrontendDto dto = new()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                Id = model.Id,
                Roles = AssignRolesToUser(model),
                Surname = model.Surname
            };
            return dto;
        }
        #endregion Mapper

    }
}
