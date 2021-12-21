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
        public async Task<IActionResult> Read()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(@"C:\Temp\Order.xlsx");
            ExcelReader reader = new ExcelReader(file);
            IExcelOrder excelOrder = await reader.LoadOrder(ExcelTypes.PL_1);


            //var file = new FileInfo(@"C:\Temp\OrderGer.xlsx");
            //ExcelReader reader = new ExcelReader(file);
            //IExcelOrder excelOrder = await reader.LoadOrder(ExcelTypes.GER_1);

            FromExcelMapper myMapper = new FromExcelMapper(_mapper);
            OrderModelFromExcelDto order = myMapper.Order(excelOrder);

            return Json(order);
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
