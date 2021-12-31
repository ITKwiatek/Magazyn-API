using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }
        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrder([FromRoute]int id)
        {
            _repo.DeleteOrderById(id);
            return true;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            OrderModel order = _repo.GetOrderWithItemsById(id);

            FrontendMapper fMapper = new(_repo);

            OrderModelFrontendDto dto = fMapper.OrderModelFrontendDto(order);

            return Json(dto);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetActiveOrders();
            var orderCards = mapper.OrderCards(orders);

            return Json(orderCards);
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInActiveOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetInActiveOrders();
            var orderCards = mapper.OrderCards(orders);

            return Json(orderCards);
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetNewOrders();
            var orderCards = mapper.OrderCards(orders);

            return Json(orderCards);
        }

        [HttpGet("finished")]
        public async Task<IActionResult> GetFinishedOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetFinishedOrders();
            var orderCards = mapper.OrderCards(orders);

            return Json(orderCards);
        }

        [HttpGet("unfinished")]
        public async Task<IActionResult> GetUnfinishedOrders()
        {
            FrontendMapper mapper = new(_repo);
            var orders = _repo.GetUnfinishedOrders();
            var orderCards = mapper.OrderCards(orders);

            return Json(orderCards);
        }

        [HttpPut("details")]
        public async Task<IActionResult> UpdateOrderDetails([FromBody] OrderModelFrontendDto orderDto)
        {
            return Json(_repo.UpdateOrderDetails(orderDto));
        }

    }
}
