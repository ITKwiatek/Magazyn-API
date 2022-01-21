
using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReleaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _repo;
        private readonly ApplicationDbContext _context;
        private readonly FrontendMapper _fMapper;

        public ReleaseController(IOrderRepository repo, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repo = repo;
            _context = context;
            _fMapper = new FrontendMapper(_repo);
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllReleases()
        {
            //return BadRequest(new Response() { Message = "asdasd", Status = "500" });
            try
            {
                var releases = _repo.GetAllReleasesWithItems();

                var dtos = _fMapper.ReleaseCardsFrontendDto(releases);

                return Ok(dtos);
            } catch (Exception e)
            {
                return BadRequest(new Response() { Message = e.Message, Status = "500" });
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReleaseById([FromRoute] int id)
        {
            var release = _repo.GetReleaseWithItemsById(id);
            var dto = _fMapper.ReleaseFrontendDto(release);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<int> Add(JObject data)
        {
            FrontendMapper fMapper = new FrontendMapper(_repo);

            OrderModelFrontendDto orderDto = data["order"].ToObject<OrderModelFrontendDto>();
            string receiverInfo = data["receiverInfo"].ToString();

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            IEnumerable<Claim> claim = identity.Claims;
            var userEmail = claim
                .Where(x => x.Type == ClaimTypes.Email)
                .FirstOrDefault().Value.ToString();


            Release release = new();
            release.OrderId = orderDto.Id;
            release.ReleasedDate = DateTime.Now;
            release.Issuer = await _userManager.FindByEmailAsync(userEmail.ToUpperInvariant());
            release.ReceiverInfo = receiverInfo;

            foreach(var item in orderDto.Items)
            {
                ReleaseItem rItem = new();
                OrderItem itemDb = _repo.GetItemById(item.Id);
                rItem.Quantity = item.CurrentQuantity - itemDb.CurrentQuantity;
                if (rItem.Quantity < 1)
                    continue;
                rItem.OrderItemId = item.Id;
                rItem.Release = release;
                release.ReleaseItems.Add(rItem);

                _repo.UpdateItem(fMapper.OrderItem(item));
            }

            if(release.ReleaseItems.Count > 0)
            {
                foreach (var rItem in release.ReleaseItems)
                {
                    rItem.Release = release;
                    _context.ReleaseItems.Add(rItem);
                }
                _context.Releases.Add(release);

                _context.SaveChanges();

                _context.Entry(release).GetDatabaseValues();
                return release.Id;
            }
            return 0;
        }

        [HttpPost("test")]
        public async Task<int> Test(int id)
        {
            return 1;
        }
    }
}
