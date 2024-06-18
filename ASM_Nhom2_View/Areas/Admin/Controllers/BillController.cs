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
            var appDbContext = _context.Bills.Include(b => b.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Bill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.BillDetails
                .Include(b => b.Product)
                .Where(m => m.BillId == id)
                .GroupBy(m => m.BillId)
                .Select(g => new 
                {
                    BillId = g.Key,
                    Product = g.Select(bd => new ProductVM
                    {
                        ProductId = bd.Product.ProductId,
                        ProductCode = bd.Product.ProductCode,
                        ProductName = bd.Product.ProductName,
                        ProductStock = bd.Product.ProductStock,
                        ProductPrice = bd.Product.ProductPrice,
                        Origin = bd.Product.Origin,
                        MachineType = bd.Product.MachineType,
                        Diameter = bd.Product.Diameter,
                        ClockType = bd.Product.ClockType,
                        Insurrance = bd.Product.Insurrance,
                        Color = bd.Product.Color,
                        BrandName = bd.Product.Brand.BrandName,
                        CategoryName = bd.Product.Category.CategoryName,
                        ProductImageList = bd.Product.ProductImageList
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (bill == null)
            {
                return NotFound();
            }

            return Json(bill);
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
