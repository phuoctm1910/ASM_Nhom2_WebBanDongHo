using Microsoft.AspNetCore.Mvc;

namespace ASM_Nhom2_View.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
