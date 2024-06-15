using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> Register([Bind("FullName,Gender,PhoneNumber,UserName,Password,BirthDate")] User user)
        {
            if (ModelState.IsValid)
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
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đăng ký tài khoản thất bại");
            }


            return View(user);

        }

        


    }
}
