using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            try
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

                var findCurrentBill = await _context.Bills.FirstOrDefaultAsync(x => x.UserID == findUser.UserID && x.Status == "Pending");
                if (findCurrentBill == null)
                {
                    findCurrentBill = new Bill
                    {
                        UserID = findUser.UserID,
                        Quantity = 0,
                        TotalAmount = 0,
                        Status = "Pending"
                    };
                    _context.Bills.Add(findCurrentBill);
                    await _context.SaveChangesAsync();
                }

                var productInBillDetails = await _context.Products.FindAsync(productId);
                if (productInBillDetails == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                var billDetail = await _context.BillDetails
                    .FirstOrDefaultAsync(bd => bd.BillId == findCurrentBill.BillId && bd.ProductId == productInBillDetails.ProductId);

                if (billDetail == null)
                {
                    // If the product is not in the bill details, add it
                    billDetail = new BillDetails
                    {
                        BillId = findCurrentBill.BillId,
                        ProductId = productInBillDetails.ProductId,
                        Quantity = 1,
                        UnitPrice = (float)productInBillDetails.ProductPrice,
                        TotalPrice = (float)productInBillDetails.ProductPrice
                    };
                    _context.BillDetails.Add(billDetail);
                }
                else
                {
                    // If the product is already in the bill details, update the quantity and total price
                    billDetail.Quantity += 1;
                    billDetail.TotalPrice += (float)productInBillDetails.ProductPrice;
                    _context.BillDetails.Update(billDetail);
                }

                // Update the bill's quantity and total amount
                findCurrentBill.Quantity += 1;
                findCurrentBill.TotalAmount += (float)productInBillDetails.ProductPrice;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product added to bill" });
            }
            catch (Exception ex)
            {
                // Return detailed exception information
                return Json(new { success = false, message = ex.ToString() });
            }
        }
    }
}
