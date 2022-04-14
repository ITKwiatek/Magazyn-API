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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Magazyn_API.Model.Auth;
using Magazyn_API.Model.User;

namespace Magazyn_API.Controllers
{
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Manager, UserRoles.Designer)]
    [ApiController]
    [Route("[controller]")]
    public class ExcelController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _eviroment;
        private readonly ApplicationDbContext _db;
        private readonly IOrderRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        public ExcelController(IMapper mapper, IOrderRepository repo, ApplicationDbContext db, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _repo = repo;
            _mapper = mapper;
            _eviroment = environment;
            _userManager = userManager;
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
                if(file.FileName.Substring(file.FileName.Length - 4) == ".xls")
                {
                    path = ExcelConverter.ConvertXLS_XLSX(new FileInfo(file.FileName));
                }

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
                        var identity = HttpContext.User.Identity as ClaimsIdentity;

                        IEnumerable<Claim> claim = identity.Claims;
                        var userEmail = claim
                            .Where(x => x.Type == ClaimTypes.Email)
                            .FirstOrDefault().Value.ToString();
                        model.ConfirmedBy = await _userManager.FindByEmailAsync(userEmail.ToUpperInvariant());

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

        [HttpGet("Add")]
        public async Task<int> Add(OrderModel order)
        {

            return 0;
        }
    }
}
