using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Controllers
{
    public class ViewModel
    {
        public IList<Product> Productfirst8 { get; set; }
        public IList<Product> Productsecond8 { get; set; }
       
    }
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var AppDbContextProductfirst8 = await _context.Products
           .Include(d => d.Category)
           .Include(d => d.Brand)
           .Take(4)
           .ToListAsync();
            var AppDbContextProductsecond8 = await _context.Products
          .Include(d => d.Category)
          .Include(d => d.Brand)
           .Skip(AppDbContextProductfirst8.Count)
          .Take(8)
          .ToListAsync();
            
            var viewModel = new ViewModel
            {
                Productfirst8 = AppDbContextProductfirst8,
                Productsecond8 = AppDbContextProductsecond8,
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
