using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Controllers
{

    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public UserController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                var hashPassBeforeCheck = PasswordHelper.GetMd5Hash(user.Password);
                var u = _context.Users.FirstOrDefault(x => x.UserName.Equals(user.UserName) && x.Password.Equals(hashPassBeforeCheck) || x.Email.Equals(user.Email) && x.Password.Equals(hashPassBeforeCheck));
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.UserName);
                    HttpContext.Session.SetString("Email", u.Email);
                    HttpContext.Session.SetString("FullName", u.FullName);
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

        [HttpGet]
        [ActionName("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var genderSelectList = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Text = "Chọn giới tính", Value = "" },
            new SelectListItem { Text = "Nam", Value = "true" },
            new SelectListItem { Text = "Nữ", Value = "false" }
        }, "Value", "Text");

            ViewBag.GenderSelectList = genderSelectList;

            return View();
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "User");
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
                Parameters = { { "prompt", "select_account" } },
                AllowRefresh = true
            };

            // Log the properties items
            foreach (var item in properties.Items)
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            }

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed");

            var state = Request.Query["state"];
            Console.WriteLine($"State parameter after redirect: {state}");

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Type,
                claim.Value
            }).ToList();

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (email != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                        UserName = email,
                        FullName = fullName,
                        Password = email + "123456",
                        RoleId = 2
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("FullName", user.FullName);

                var claimsIdentity = new ClaimsIdentity(authenticateResult.Principal.Identities.FirstOrDefault().Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "User");
        }

        


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        public async Task<IActionResult> Register([Bind("FullName,Gender,PhoneNumber,UserName,Password,BirthDate")] User user)
        {
            if (!ModelState.IsValid)
            {
                var newUser = new User()
                {
                    FullName = user.FullName,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Password = user.Password,
                    BirthDate = user.BirthDate,
                    RoleId = 2
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
                return View(model);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Email không tồn tại.";
                return View(model);
            }

            string newPassword = PasswordHelper.GeneratePassword(8);
            user.Password = PasswordHelper.GetMd5Hash(newPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Quên mật khẩu", $"Mật khẩu mới của bạn là: {newPassword}");

            ViewBag.Message = "Mật khẩu mới đã được gửi qua email.";
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


            return View(user);


            user.Password = newPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đổi mật khẩu thành công." });
        }

        






    }
}
