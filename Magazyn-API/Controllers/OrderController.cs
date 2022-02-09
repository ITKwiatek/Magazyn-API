using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Magazyn_API.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    //[EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager)]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrder([FromRoute]int id)
        {
            _repo.DeleteOrderById(id);
            return true;
        }
        [AuthorizeRoles(UserRoles.Admin,UserRoles.Manager, UserRoles.Storekeeper, UserRoles.Designer)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            OrderModel order = _repo.GetOrderWithItemsById(id);
            if (order == null)
                return BadRequest("Nie znaleziono");

            FrontendMapper fMapper = new(_repo);

            OrderModelFrontendDto dto = fMapper.OrderModelFrontendDto(order);

            dto.Items = dto.Items.OrderBy(i => i.Component.Supplier).ToList();

            return Json(dto);
        }
        [HttpGet("active")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager, UserRoles.Storekeeper, UserRoles.Designer)]
        public async Task<IActionResult> GetActiveOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetActiveOrders();
            var orderCards = mapper.OrderCards(orders);

            orderCards = orderCards.OrderBy(o => o.DateToRelease).ToList();

            return Json(orderCards);
        }

        [HttpGet("inactive")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager)]
        public async Task<IActionResult> GetInActiveOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetInActiveOrders();
            var orderCards = mapper.OrderCards(orders);

            orderCards = orderCards.OrderBy(o => o.Id).ToList();

            return Json(orderCards);
        }

        [HttpGet("new")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager, UserRoles.Storekeeper, UserRoles.Designer)]
        public async Task<IActionResult> GetNewOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetNewOrders();
            var orderCards = mapper.OrderCards(orders);

            orderCards = orderCards.OrderBy(o => o.DateToRelease).ToList();

            return Json(orderCards);
        }

        [HttpGet("finished")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager, UserRoles.Storekeeper, UserRoles.Designer)]
        public async Task<IActionResult> GetFinishedOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetFinishedOrders();
            var orderCards = mapper.OrderCards(orders);

            orderCards.OrderByDescending(o => o.DateToRelease);

            return Json(orderCards);
        }

        [HttpGet("unfinished")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager, UserRoles.Storekeeper, UserRoles.Designer)]
        public async Task<IActionResult> GetUnfinishedOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetUnfinishedOrders();
            var orderCards = mapper.OrderCards(orders);

            orderCards.OrderByDescending(o => o.DateToRelease);

            return Json(orderCards);
        }

        [HttpPut("details")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager)]
        public async Task<IActionResult> UpdateOrderDetails([FromBody] OrderModelFrontendDto orderDto)
        {
            return Json(_repo.UpdateOrderDetails(orderDto));
        }

    }
}
