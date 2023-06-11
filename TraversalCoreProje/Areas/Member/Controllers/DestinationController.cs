using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Areas.Member.Controllers
{
    public class DestinationController : Controller
    {
        DestinationManager _destinationManager = new DestinationManager(new EfDestinationDal());
        [AllowAnonymous]
        [Area("Member")]
        public IActionResult Index()
        {
            var values = _destinationManager.TGetList();
            return View(values);
        }
    }
}
