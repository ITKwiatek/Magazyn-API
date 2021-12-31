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
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Magazyn_API.Service;

namespace Magazyn_API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ExcelController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _eviroment;
        private readonly ApplicationDbContext _db;
        private readonly IOrderRepository _repo;
        public ExcelController(IMapper mapper, IOrderRepository repo, ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _repo = repo;
            _mapper = mapper;
            _eviroment = environment;
        }
        [HttpPost("")]
        public async Task<IActionResult> Read([FromBody]JObject data)
        {
            string path = data["path"].ToString();
            ExcelTypes excelType = data["excelType"].ToObject<ExcelTypes>();     
            try
            {
                ExcelReader reader = new ExcelReader(path);
                IExcelOrder excelOrder = await reader.LoadOrder(excelType);
                FromExcelMapper myMapper = new FromExcelMapper(_mapper);
                OrderModelFromExcelDto order = myMapper.Order(excelOrder);

                return Json(order);
            } catch(Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost("Read/{excelType}")]
        public async Task<IActionResult> ReadExcel(IFormFile file, [FromRoute] ExcelTypes excelType)
        {
            string path = "";
            if (file != null)
            {
                FileHelper helper = new FileHelper("C:\\temp\\delete\\");
                await helper.DeleteFilesInDir();
                path = await helper.SaveFile(file);
                if (!string.IsNullOrWhiteSpace(path)) 
                {
                    try
                    {
                        ExcelReader reader = new ExcelReader(path);
                        IExcelOrder excelOrder = await reader.LoadOrder(excelType);

                        FromExcelMapper mapper = new FromExcelMapper(_mapper);
                        OrderModelFromExcelDto orderDto = mapper.Order(excelOrder);

                        OrderMapper oMapper = new OrderMapper(_repo);
                        OrderModel model = oMapper.Order(orderDto);

                        _repo.SaveOrder(model);
                        int id = _repo.GetOrderId(model);

                        return Ok(id);
                    } catch (Exception e)
                    {
                        return Json(e.Message);
                    }
                }
            }
            return Json("Serwer nie był w stanie wczytać pliku...");
        }

        [HttpPost("Save")]
        public async Task<bool> Save(OrderModelFromExcelDto orderDto)
        {
            OrderMapper oM = new OrderMapper(_repo);
            OrderModel order = oM.Order(orderDto);
            var response = _repo.SaveOrder(order);
            return response;
        }

        [HttpGet("Add")]
        public async Task<int> Add(OrderModel order)
        {

            return 0;
        }
    }
}
