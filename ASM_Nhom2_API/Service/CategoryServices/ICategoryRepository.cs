using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.CategoryServices
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task AddCategoryAsync(CategoryVM categoryVM);
        Task UpdateCategoryAsync(int categoryId, CategoryVM categoryVM);
        Task DeleteCategoryAsync(int categoryId);
    }
}
