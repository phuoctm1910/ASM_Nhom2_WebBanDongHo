using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Controllers
{
    public class UserController : Controller
    {

        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = _context.Users.Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.UserName.ToString());
                    HttpContext.Session.SetString("Password", u.Password.ToString());
                    HttpContext.Session.SetString("FullName", u.FullName.ToString());
                    if (u.RoleId == 2)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });

                    }
                }
            }
            return View();
        }
        [ActionName("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Changeinfo()
        {
            return View();
        }



        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }




        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
            {
                return Json(new { success = false, message = "Mật khẩu mới và xác nhận mật khẩu không khớp." });
            }

            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                return Json(new { success = false, message = "Người dùng chưa đăng nhập." });
            }

            var user = _context.Users
                .Where(u => u.UserName.Equals(userName) && u.Password.Equals(currentPassword))
                .FirstOrDefault();

            if (user == null)
            {
                return Json(new { success = false, message = "Mật khẩu hiện tại không đúng." });
            }

            user.Password = newPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đổi mật khẩu thành công." });
        }





    }
}
