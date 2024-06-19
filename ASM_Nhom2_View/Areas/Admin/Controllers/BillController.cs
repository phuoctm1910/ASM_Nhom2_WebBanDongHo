using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Models;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authentication]
    public class BillController : Controller
    {
        private readonly AppDbContext _context;

        public BillController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Bill
        public async Task<IActionResult> Index()
        {
            var bill = await _context.Bills.Include(b => b.User).ToListAsync();

            return View(bill);


        }
        [HttpGet]
        public async Task<IActionResult> GetBillDetail(int billId, int userId)
        {
            var billDetail = await _context.Bills
                .Where(b => b.BillId == billId && b.UserID == userId)
                .Select(b => new
                {
                    b.BillId,
                    b.UserID,
                    UserName = b.User.FullName,
                    b.Quantity,
                    b.TotalAmount,
                    b.Status,
                    b.RecipientName,
                    b.RecipientPhoneNumber,
                    b.RecipientAddress,
                    b.PaymentMethod
                })
                .FirstOrDefaultAsync();

            if (billDetail == null)
            {
                return NotFound();
            }

            var totalAllUnit = await _context.BillDetails
                .Where(bd => bd.BillId == billId)
                .SumAsync(bd => bd.TotalPrice);

            return Json(new { billDetail, totalAllUnit });
        }

        // GET: Admin/Bill/Details/5
        [HttpGet]
        public async Task<IActionResult> GetProductInBill(int id)
        {
            var billDetails = await _context.BillDetails
                .Where(bd => bd.BillId == id)
                .Include(bd => bd.Product)
                .GroupBy(bd => new { bd.ProductId, bd.Product.ProductName, bd.UnitPrice })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.ProductName,
                    g.Key.UnitPrice,
                    Quantity = g.Sum(bd => bd.Quantity),
                    TotalPrice = g.Sum(bd => bd.TotalPrice)
                })
                .ToListAsync();

            if (!billDetails.Any())
            {
                ViewBag.Error = "Không tìm thấy thông tin chi tiết của bill này.";
                return View("Error");
            }

            return Json(billDetails);
        }



        //// GET: Admin/Bill/Create
        //public IActionResult Create()
        //{
        //    ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FullName");
        //    return View();
        //}

        //// POST: Admin/Bill/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BillId,UserID,Quantity,TotalAmount,Status")] Bill bill)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(bill);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FullName", bill.UserID);
        //    return View(bill);
        //}

        //// GET: Admin/Bill/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var bill = await _context.Bills.FindAsync(id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FullName", bill.UserID);
        //    return View(bill);
        //}

        //// POST: Admin/Bill/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BillId,UserID,Quantity,TotalAmount,Status")] Bill bill)
        //{
        //    if (id != bill.BillId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(bill);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BillExists(bill.BillId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FullName", bill.UserID);
        //    return View(bill);
        //}

        //// GET: Admin/Bill/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var bill = await _context.Bills
        //        .Include(b => b.User)
        //        .FirstOrDefaultAsync(m => m.BillId == id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(bill);
        //}

        //// POST: Admin/Bill/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var bill = await _context.Bills.FindAsync(id);
        //    _context.Bills.Remove(bill);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool BillExists(int id)
        //{
        //    return _context.Bills.Any(e => e.BillId == id);
        //}
    }
}
