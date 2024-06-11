using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.BrandServices
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;

        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int brandId)
        {
            return await _context.Brands.FindAsync(brandId);
        }

        public async Task AddBrandAsync(BrandVM brandVM)
        {
            var brand = new Brand
            {
                BrandName = brandVM.BrandName
            };

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(int brandId, BrandVM brandVM)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand != null)
            {
                brand.BrandName = brandVM.BrandName;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBrandAsync(int brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }
    }
}
