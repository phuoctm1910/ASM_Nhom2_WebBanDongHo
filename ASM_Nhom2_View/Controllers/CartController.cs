using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

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

            // Return the pending bill to the view
            return View(pendingBill);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int billDetailId)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (findUser == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var billDetail = await _context.BillDetails.FirstOrDefaultAsync(bd => bd.Id == billDetailId && bd.Bill.UserID == findUser.UserID);
            if (billDetail == null)
            {
                return Json(new { success = false, message = "Bill detail not found" });
            }

            var bill = await _context.Bills.FirstOrDefaultAsync(b => b.BillId == billDetail.BillId);
            if (bill == null)
            {
                return Json(new { success = false, message = "Bill not found" });
            }

            // Update total quantity and total amount of Bill
            bill.Quantity -= billDetail.Quantity;
            bill.TotalAmount -= billDetail.TotalPrice;

            // Remove BillDetail from database
            _context.BillDetails.Remove(billDetail);

            // Save changes
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Product removed from bill" });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeProductQuantity(int billDetailId, int change)
        {
            var billDetail = await _context.BillDetails.FirstOrDefaultAsync(b => b.Id == billDetailId);
            if (billDetail == null)
            {
                return Json(new { success = false, message = "Product not found in the bill" });
            }

            billDetail.Quantity += change;
            if (billDetail.Quantity <= 0)
            {
                _context.BillDetails.Remove(billDetail);
            }
            else
            {
                billDetail.TotalPrice = billDetail.Quantity * billDetail.UnitPrice;
                _context.BillDetails.Update(billDetail);
            }

            await _context.SaveChangesAsync();

            // Update the total amount in the bill
            var bill = await _context.Bills.FirstOrDefaultAsync(b => b.BillId == billDetail.BillId);
            if (bill != null)
            {
                bill.Quantity = await _context.BillDetails
                    .Where(bd => bd.BillId == bill.BillId)
                    .SumAsync(bd => bd.Quantity);

                bill.TotalAmount = await _context.BillDetails
                    .Where(bd => bd.BillId == bill.BillId)
                    .SumAsync(bd => bd.TotalPrice);

                _context.Bills.Update(bill);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Product quantity updated successfully" });
        }

    }
}
