using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ASM_Nhom2_View.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get user email from session
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User");
            }

            // Retrieve user from database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Redirect to login if user not found
                return RedirectToAction("Login", "User");
            }

            var pendingBill = await _context.Bills
                .Include(b => b.BillDetails)
                .ThenInclude(bd => bd.Product)
                .FirstOrDefaultAsync(b => b.UserID == user.UserID && b.Status == "Pending");

            if (pendingBill == null)
            {
                return View();
            }

            // Pass the pending bill to the view
            return View(pendingBill);
        }

        [HttpPost]
        public async Task<IActionResult> CompletePurchase(string fullName, string phone, string email, string address, string paymentMethod)
        {
            // Get user email from session
            var sessionEmail = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(sessionEmail))
            {
                return RedirectToAction("Login", "User");
            }

            // Retrieve user from database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == sessionEmail);
            if (user == null)
            {
                // Redirect to login if user not found
                return RedirectToAction("Login", "User");
            }

            var pendingBill = await _context.Bills
                .Include(b => b.BillDetails)
                .FirstOrDefaultAsync(b => b.UserID == user.UserID && b.Status == "Pending");

            if (pendingBill == null)
            {
                return RedirectToAction("Index");
            }

            // Create a new order
            var order = new Order
            {
                UserID = user.UserID,
                FullName = fullName,
                Phone = phone,
                Email = email,
                Address = address,
                PaymentMethod = paymentMethod,
                OrderDate = DateTime.Now,
                TotalAmount = pendingBill.TotalAmount
            };

            // Save the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Update bill details
            pendingBill.Status = "Completed"; // Mark bill as completed
            _context.Bills.Update(pendingBill);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
