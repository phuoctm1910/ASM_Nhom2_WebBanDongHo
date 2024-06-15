using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.ProductServices
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _context.Products
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            product.ProductImages = JsonConvert.SerializeObject(product.ProductImages);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProd = await _context.Products.FindAsync(id);
            if (existingProd == null) return null;

            existingProd.ProductCode = product.ProductCode;
            existingProd.ProductName = product.ProductName;
            existingProd.ProductStock = product.ProductStock;
            existingProd.ProductPrice = product.ProductPrice;
            existingProd.CategoryId = product.CategoryId;
            existingProd.ProductImages = JsonConvert.SerializeObject(product.ProductImages);
            existingProd.Origin = product.Origin;
            existingProd.BrandId = product.BrandId;
            existingProd.ClockType = product.ClockType;
            existingProd.Insurrance = product.Insurrance;
            existingProd.Diameter = product.Diameter;
            existingProd.Color = product.Color;
            existingProd.MachineType = product.MachineType;
            await _context.SaveChangesAsync();
            return existingProd;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
