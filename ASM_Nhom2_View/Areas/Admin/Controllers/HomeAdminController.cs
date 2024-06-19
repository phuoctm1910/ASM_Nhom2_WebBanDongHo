using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(1)]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
