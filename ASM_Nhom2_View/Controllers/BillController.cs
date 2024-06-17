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
                return Json(new { success = false, message = "User not logged in" });
            }

            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (findUser == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

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
                return Json(new { success = false, message = "Product not found" });
            }

            var billDetail = await _context.BillDetails
                .FirstOrDefaultAsync(bd => bd.BillId == currentBill.BillId && bd.ProductId == product.ProductId);

            if (billDetail == null)
            {
                // If the product is not in the bill details, add it
                billDetail = new BillDetails
                {
                    BillId = currentBill.BillId,
                    ProductId = product.ProductId,
                    Quantity = 1,
                    UnitPrice = (float)product.ProductPrice,
                    TotalPrice = (float)product.ProductPrice
                };
                _context.BillDetails.Add(billDetail);
            }
            else
            {
                // If the product is already in the bill details, update the quantity and total price
                billDetail.Quantity += 1;
                billDetail.TotalPrice += (float)product.ProductPrice;
                _context.BillDetails.Update(billDetail);
            }

            // Update the bill's quantity and total amount
            currentBill.Quantity += 1;
            currentBill.TotalAmount += (float)product.ProductPrice;

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Product added to bill" });
        }
    }
}
