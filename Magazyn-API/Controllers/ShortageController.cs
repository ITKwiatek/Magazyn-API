using Magazyn_API.Data;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Shortage;
using Magazyn_API.Model.User;
using Magazyn_API.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager, UserRoles.Storekeeper, UserRoles.Designer)]
    [ApiController]
    [Route("[controller]")]
    public class ShortageController : Controller
    {
        private readonly IOrderRepository _repo;

        public ShortageController(IOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("projects")]
        public async Task<List<string>> GetAllProjectNames()
        {
            List<string> projects = _repo.GetAllProjectNames();

            return projects;
        }

        [HttpGet("devices")]
        public async Task<List<string>> GetAllDevices()
        {
            List<string> devices = _repo.GetAllDeviceNames();
            return devices;
        }

        [HttpGet("groups")]
        public async Task<List<string>> GetAllGroups()
        {
            List<string> groups = _repo.GetAllGroupNames();

            return groups;
        }

        [HttpPost()]
        public async Task<List<ComponentShortage>> GetShortages([FromBody] JObject data)
        {
            List<string> projects = data["projects"].ToObject<List<string>>();
            List<string> groups = data["groups"].ToObject<List<string>>();
            List<string> devices = data["devices"].ToObject<List<string>>();

            ShortageFilterService service = new(_repo, projects, groups, devices);     

            List<ComponentShortage> shortages = service.GetShortages();

            return shortages;
        }
    }
}
