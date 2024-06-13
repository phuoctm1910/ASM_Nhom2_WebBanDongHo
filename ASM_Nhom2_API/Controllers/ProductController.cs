using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productService;
        private readonly AppDbContext _context;

        public ProductsController(IProductRepository productService, AppDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVM>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVM>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductVM productVM)
        {
            var category = await _context.Categories.FindAsync(productVM.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId.");
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductCode == productVM.ProductCode);
            if (existingProduct != null)
            {
                return BadRequest("Product with the same code already exists.");
            }
            else
            {
                await _productService.AddProductAsync(productVM);
            }

            return CreatedAtAction(nameof(GetProduct), new { id = productVM.ProductId }, productVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(product.ProductId);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<ProductVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId.");
            }

            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == product.ProductCode && p.ProductId != id);
            if (existingProduct != null)
            {
                return BadRequest("Product with the same code already exists.");
            }

            patchDoc.ApplyTo(product, ModelState);

            if (!TryValidateModel(product))
            {
                return ValidationProblem(ModelState);
            }

            await _productService.UpdateProductAsync(id, product);

            return Ok(product);
        }

    }
}
