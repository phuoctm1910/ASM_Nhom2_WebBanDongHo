using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;



namespace ASM_Nhom2_View.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([Bind("FullName,PhoneNumber,UserName,Password,BirthDay,Gender,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                ModelState.AddModelError(string.Empty, "Đăng ký tài khoản thành công");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đăng ký tài khoản thất bại");
            }
           
            return View(user);
        }


    }
}
