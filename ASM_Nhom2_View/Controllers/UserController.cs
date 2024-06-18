using ASM_Nhom2_View.Models;
using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting;


namespace ASM_Nhom2_View.Controllers
{
    public class UserController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _web;

        public UserController(AppDbContext context, IWebHostEnvironment web)
        {
            _context = context;
            _web = web;
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

        [HttpGet]
        public async Task<IActionResult> Changeinfo()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return View();
            }

            // Retrieve the user from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return NotFound();
            }

            // Pass the user data to the view
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Changeinfo(User change, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var userName = HttpContext.Session.GetString("UserName");

                if (userName == null)
                {
                    return View();
                }

                var userToUpdateInfo = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

                if (userToUpdateInfo == null)
                {
                    return NotFound();
                }

                userToUpdateInfo.UserName = change.UserName;
                userToUpdateInfo.FullName = change.FullName;
                userToUpdateInfo.PhoneNumber = change.PhoneNumber;
                userToUpdateInfo.Email = change.Email;
                userToUpdateInfo.Gender = change.Gender;
                userToUpdateInfo.BirthDate = change.BirthDate;
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_web.WebRootPath, "uploads", fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    userToUpdateInfo.Image = fileName;
                }
                _context.Users.Update(userToUpdateInfo);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("FullName", userToUpdateInfo.FullName);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Cập nhật tài khoản thất bại");
            }

            return View(change);
        }




    }
}
