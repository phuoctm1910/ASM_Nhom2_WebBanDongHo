using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM_Nhom2_View.Data;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(1)]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Users.Include(u => u.Role).Where(c => c.RoleId == 2);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var user = await _context.Users
            //    .Include(u => u.Role)
            //    .FirstOrDefaultAsync(m => m.UserID == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            //return View(user);
            return RedirectToAction("Index", "HomeAdmin");
        }

        // GET: Admin/User/Create
        public IActionResult Create()
        {
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            //return View();
            return RedirectToAction("Index", "HomeAdmin");

        }

        // POST: Admin/User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,FullName,Gender,BirthDate,PhoneNumber,Image,Email,UserName,Password,RoleId")] User user)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(user);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
            //return View(user);
            return RedirectToAction("Index", "HomeAdmin");

        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,FullName,Gender,BirthDate,PhoneNumber,Image,Email,UserName,Password,RoleId")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Admin/User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var user = await _context.Users.FindAsync(id);
            //_context.Users.Remove(user);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "HomeAdmin");

        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
