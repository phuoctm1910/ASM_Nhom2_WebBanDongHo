using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ASM_Nhom2_View.Controllers
{
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
      
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Brand).Include(p => p.Category).ToList();
            return View(products);
        }
    }
}
