using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<ProductVM>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductVM
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    ProductStock = p.ProductStock,
                    ProductPrice = p.ProductPrice,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                }).ToListAsync();
        }

        public async Task<ProductVM> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductVM
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    ProductStock = p.ProductStock,
                    ProductPrice = p.ProductPrice,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                }).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(ProductVM productVM)
        {
            var product = new Product
            {
                ProductCode = productVM.ProductCode,
                ProductName = productVM.ProductName,
                ProductStock = productVM.ProductStock,
                ProductPrice = productVM.ProductPrice,
                CategoryId = productVM.CategoryId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(int productId, ProductVM productVM)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.ProductCode = productVM.ProductCode;
                product.ProductName = productVM.ProductName;
                product.ProductStock = productVM.ProductStock;
                product.ProductPrice = productVM.ProductPrice;
                product.CategoryId = productVM.CategoryId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
