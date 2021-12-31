using Magazyn_API.Data;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Excel;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FrontendDto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class VirtualController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IOrderRepository _repo;

        public VirtualController(IOrderRepository repo, ApplicationDbContext db)
        {
            _db = db;
            _repo = repo;
        }
        [HttpPost("")]
        public async Task<bool> CreateVirtualOrder(JObject data)
        {
            string name = data["name"].ToString();
            if (string.IsNullOrWhiteSpace(name))
                return false;
            List<int> orderIds = new();
            orderIds = data["orderIds"].ToObject<List<int>>();

            FrontendMapper fMapper = new(_repo);
            VirtualOrderModel model = new();
            List<OrderModel> orders = new();
            List<VirtualItem> vItems = new();
            model.Name = name;
            model.CreatedDate = DateTime.Now;
            _db.VirtualOrders.Add(model);
            _db.SaveChanges();
            foreach (int id in orderIds)
            {
                OrderModel order = _repo.GetOrderWithItemsById(id);
                orders.Add(order);

                foreach(var item in order.OrderItems)
                {
                    if(vItems.Exists(i => i.ComponentId == item.ComponentId))
                    {
                        var duplicate = vItems.Find(i => i.ComponentId == item.ComponentId);
                        duplicate.RequiredQuantity += item.RequiredQuantity - item.CurrentQuantity;
                        _db.VirtualItems.Update(duplicate);
                        _db.SaveChanges();
                    } else
                    {
                        VirtualItem vItem = new();
                        vItem.Component = item.Component;
                        vItem.ComponentId = item.ComponentId;
                        vItem.RequiredQuantity = item.RequiredQuantity - item.CurrentQuantity;
                        vItem.VirtualOrder = model;

                        vItems.Add(vItem);
                        _db.VirtualItems.Add(vItem);
                        _db.SaveChanges();
                    }
                }

                model.OrderItems = vItems;
            }
            foreach(int id in orderIds)
            {
                OrderModel orderWithoutItems = _repo.GetOrderWithoutItemsById(id);
                model.Orders.Add(orderWithoutItems);
            }
            _db.VirtualOrders.Update(model);
            _db.SaveChanges();
            
            return true;
        }

        [HttpGet("{id}")]
        public async Task<VirtualOrderFrontendDto> GetVirtualOrder([FromBody] int id)
        {

            return new VirtualOrderFrontendDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetVirtualOrders()
        {
            List<VirtualOrderModel> orders = _repo.GetAllVirtualOrdersWithItems();
            FrontendMapper fMapper = new(_repo);
            List<VirtualOrderCard> dtos = fMapper.VirtualOrdersCards(orders);
            return Json(dtos);
        }

    }
}
