using BusinessLayer.Abstract;
using ClosedXML.Excel;
using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
    /// <summary>
    /// EPPLUS Nuget Package Kurulmalıdır 
    /// CloseXML Nuget Package Kurulmalıdır 
    /// </summary>
    public class ExcelController : Controller
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public List<DestinationModel> DestinationList()
        {
            List<DestinationModel> destinationModels = new List<DestinationModel>();
            using (var c = new Context())
            {
                destinationModels = c.Destinations.Select(x => new DestinationModel
                {
                    City = x.City,
                    DayNight = x.DayNight,
                    Price = x.Price,
                    Capacity = x.Capacity
                }).ToList();
            }
            return destinationModels;
        }
        public IActionResult StaticExcelReport()
        {
            return File(_excelService.ExcelList(DestinationList()), "application/vnd.openxmlformats-offiedocuments.spreadsheetml.sheet", "YeniExcel.xlsx");
        }
        public IActionResult DestinationExcelReport()
        {
            return File(_excelService.ExcelList(DestinationList()), "application/vnd." +
        "openxmlformats-offiedocuments.spreadsheetml.sheet", "YeniListe.xlsx");
        }
    }
}
