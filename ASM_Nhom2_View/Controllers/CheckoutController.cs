using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using ASM_Nhom2_View.Models;

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
        [Authentication]
        public async Task<IActionResult> CompletePurchase(string fullName, string phone,  string address, string paymentMethod)
        {
            // Xử lý thông tin nhận hàng và lưu vào cơ sở dữ liệu
            // Ví dụ:
            var sessionEmail = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(sessionEmail))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == sessionEmail);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var pendingBill = await _context.Bills
                .Include(b => b.BillDetails)
                .FirstOrDefaultAsync(b => b.UserID == user.UserID && b.Status == "Pending");

            if (pendingBill == null)
            {
                return RedirectToAction("Index");
            }

            // Cập nhật thông tin nhận hàng
            pendingBill.RecipientName = fullName;
            pendingBill.RecipientPhoneNumber = phone;
            pendingBill.RecipientAddress = address;
            pendingBill.PaymentMethod = paymentMethod;
            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Bills.Update(pendingBill);
            await _context.SaveChangesAsync();

            // Cập nhật trạng thái đơn hàng thành "Completed"
            pendingBill.Status = "Completed";
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang chủ hoặc trang cảm ơn
            return RedirectToAction("Index", "Home");
        }
    }
}
