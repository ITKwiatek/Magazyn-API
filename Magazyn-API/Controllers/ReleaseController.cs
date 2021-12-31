
using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ReleaseController : Controller
    {
        private readonly IOrderRepository _repo;
        private readonly ApplicationDbContext _context;

        public ReleaseController(IOrderRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }
        [HttpGet("/{id}")]
        public async Task<IActionResult> GetReleaseById([FromRoute] int id)
        {

            return Json("");
        }
        [HttpPost]
        public async Task<int> Add(JObject data)
        {
            FrontendMapper fMapper = new FrontendMapper(_repo);

            OrderModelFrontendDto orderDto = data["order"].ToObject<OrderModelFrontendDto>();


            Release release = new();
            release.OrderId = orderDto.Id;
            release.ReceiveDate = DateTime.Now;

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
