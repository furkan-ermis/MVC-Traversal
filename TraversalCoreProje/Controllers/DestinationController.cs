using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Controllers
{
    public class DestinationController : Controller
    {
        DestinationManager destinationManager = new DestinationManager(new EfDestinationDal());

        public IActionResult Index()
        {
            var values = destinationManager.TGetList();
            return View(values);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.i = id;
            var value = destinationManager.TGetByID(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult Details(Destination destination)
        {
            return View();
        }
    }
}
