using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.CategoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryService;
        private readonly AppDbContext _context;
        public CategoryController(ICategoryRepository categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] CategoryVM categoryVM)
        {
            await _categoryService.AddCategoryAsync(categoryVM);
            var createdCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryVM.CategoryName);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, createdCategory);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryVM categoryVM)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateCategoryAsync(id, categoryVM);

            var updatedCategories = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(updatedCategories);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(id);

            var afterdeletedCategories = await _categoryService.GetAllCategoriesAsync();
            return Ok(afterdeletedCategories);
        }
    }
}
