using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Http;

namespace ASM_Nhom2_View.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get user email from session
            var email = HttpContext.Session.GetString("Email");
            //if (string.IsNullOrEmpty(email))
            //{
            //    return RedirectToAction("Login", "User");
            //}

            // Retrieve user from database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            //if (user == null)
            //{
            //    // Redirect to login if user not found
            //    return RedirectToAction("Login", "User");
            //}

            // Retrieve pending bill for the user
            var pendingBill = await _context.Bills
                .Include(b => b.BillDetails)
                .ThenInclude(bd => bd.Product)
                .FirstOrDefaultAsync(b => b.UserID == user.UserID && b.Status == "Pending");

            if (pendingBill == null)
            {
                return View();
            }

            // Return the pending bill to the view
            return View(pendingBill);
        }
    }
}
