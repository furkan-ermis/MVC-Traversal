using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TraversalCoreProje.Areas.Member.Controllers
{
    [Area("Member")]
    [AllowAnonymous] // ŞİMDİLİK
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
