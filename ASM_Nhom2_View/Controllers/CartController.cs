using Microsoft.AspNetCore.Mvc;

namespace ASM_Nhom2_View.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
