using Magazyn_API.Model.Excel.PL_1;
using Magazyn_API.Model.Excel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Magazyn_API.Model.Order;
using Magazyn_API.AutoMapper;
using Magazyn_API.Data;
using Newtonsoft.Json;
using Magazyn_API.Model.Order.FromExcelDto;
using Magazyn_API.Mappers;
using Magazyn_API.Model.Mappers;
using Newtonsoft.Json.Linq;

namespace Magazyn_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExcelController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly IOrderRepository _repo;
        public ExcelController(IMapper mapper, IOrderRepository repo, ApplicationDbContext db)
        {
            _db = db;
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet("")]
        public async Task<IActionResult> Read([FromBody]JObject data)
        {
            string path = data["path"].ToString();
            ExcelTypes excelType = data["excelType"].ToObject<ExcelTypes>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                ExcelReader reader = new ExcelReader(new FileInfo(path));
                IExcelOrder excelOrder = await reader.LoadOrder(excelType);
                FromExcelMapper myMapper = new FromExcelMapper(_mapper);
                OrderModelFromExcelDto order = myMapper.Order(excelOrder);

                return Json(order);
            } catch(Exception e)
            {
                return Json(e.Message);
            }
        }
        [HttpPost("Save")]
        public async Task<bool> Save(OrderModelFromExcelDto orderDto)
        {
            OrderMapper oM = new OrderMapper(_repo);
            OrderModel order = oM.Order(orderDto);
            var response = _repo.SaveOrder(order);
            return response;
        }
    }
}
