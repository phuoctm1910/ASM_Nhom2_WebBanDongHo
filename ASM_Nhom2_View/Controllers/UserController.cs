using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
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
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = _context.Users.FirstOrDefault(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password));
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.UserName);
                    HttpContext.Session.SetString("Password", u.Password);
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
            return View();
        }

        [HttpGet("/signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed");

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
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "User");
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
                Parameters = { { "prompt", "select_account" } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
    }
}
