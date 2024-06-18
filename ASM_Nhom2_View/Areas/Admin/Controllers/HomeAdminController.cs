using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authentication]
    public class HomeAdminController : Controller
    {
        [Route("HomeAdmin/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
