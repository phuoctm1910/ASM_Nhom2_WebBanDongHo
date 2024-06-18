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

            var billDetail = await _context.BillDetails.FirstOrDefaultAsync(bd => bd.Id == billDetailId);

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

            // Define your limit here
            var product = _context.Products.FirstOrDefault(c => c.ProductId == billDetail.ProductId);
            int maxAllowedQuantity = product.ProductStock;  // Adjust this limit as per your requirement

            if (change < 0 ||  change > maxAllowedQuantity)
            {
                return Json(new { success = false, message = $"Cannot decrease quantity or exceed maximum of {maxAllowedQuantity}" });
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
        [HttpPost]
        public async Task<IActionResult> UpdateTotalAmount(int billId, float lastTotalAmount)
        {
            var billUpdateTotalAmount = await _context.Bills.FirstOrDefaultAsync(b => b.BillId == billId);
            if (billUpdateTotalAmount == null)
            {
                return Json(new { success = false, message = "Bill Was Not Found" });
            }

            billUpdateTotalAmount.TotalAmount += lastTotalAmount - billUpdateTotalAmount.TotalAmount;

            if (billUpdateTotalAmount.TotalAmount < lastTotalAmount)
            {
                return Json(new { success = false, message = "Fail To Update Last Total Amount On Bill" });

            }
            else
            {
                _context.Bills.Update(billUpdateTotalAmount);
                await _context.SaveChangesAsync();

            }
            return Json(new { success = true, message = "Last Total Amount On Bill Was Update" });
        }
    }
}
