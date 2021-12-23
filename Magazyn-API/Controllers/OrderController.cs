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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            OrderModel order = _repo.GetOrderById(id);

            return Json(order);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetOrders()
        {
            FrontendMapper mapper = new();
            var orders = _repo.GetAllOrders();
            var orderCards = mapper.OrderCards(orders);

            return Json(orderCards);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderModel order)
        {
            return Json(_repo.UpdateOrder(order));
        }
    }
}
