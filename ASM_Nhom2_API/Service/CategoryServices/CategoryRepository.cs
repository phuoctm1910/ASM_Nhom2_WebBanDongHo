using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.CategoryServices
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task AddCategoryAsync(CategoryVM categoryVM)
        {
            var category = new Category
            {
                CategoryName = categoryVM.CategoryName
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(int categoryId, CategoryVM categoryVM)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                category.CategoryName = categoryVM.CategoryName;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
