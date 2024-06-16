using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Controllers
{
    public class BillController : Controller
    {

        private readonly AppDbContext _context;
        public BillController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBill(int productId)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User");
            }

            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            var currentBill = await _context.Bills.FirstOrDefaultAsync(b => b.UserID == findUser.UserID && b.Status == "Pending");
            if (currentBill == null)
            {
                currentBill = new Bill
                {
                    UserID = findUser.UserID,
                    Quantity = 0,
                    TotalAmount = 0,
                    Status = "Pending"
                };
                _context.Bills.Add(currentBill);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                // Handle case where product is not found
                return NotFound();
            }

            // Create new BillDetail with default quantity of 1
            var newBillDetail = new BillDetails
            {
                BillId = currentBill.BillId,
                ProductId = product.ProductId,
                Quantity = 1,
                UnitPrice = (float)product.ProductPrice,
                TotalPrice = (float)product.ProductPrice
            };

            // Add BillDetail to database
            _context.BillDetails.Add(newBillDetail);

            // Update total quantity and total amount of Bill
            currentBill.Quantity += 1;
            currentBill.TotalAmount += newBillDetail.TotalPrice;

            // Save changes
            await _context.SaveChangesAsync();

            return Ok(); 
        }
    }
}
