using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.BrandServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandService;
        private readonly AppDbContext _context;
        public BrandController(IBrandRepository brandService, AppDbContext context)
        {
            _brandService = brandService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            var categories = await _brandService.GetAllBrandAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult> AddBrand([FromBody] BrandVM brandVM)
        {
            await _brandService.AddBrandAsync(brandVM);
            var createdBrand = await _context.Brands.FirstOrDefaultAsync(c => c.BrandName == brandVM.BrandName);
            return CreatedAtAction(nameof(GetBrand), new { id = createdBrand.BrandId }, createdBrand);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandVM brandVM)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            await _brandService.UpdateBrandAsync(id, brandVM);

            var updatedBrands = await _brandService.GetBrandByIdAsync(id);
            return Ok(updatedBrands);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            await _brandService.DeleteBrandAsync(id);

            var afterdeletedBrands = await _brandService.GetAllBrandAsync();
            return Ok(afterdeletedBrands);
        }
    }
}
