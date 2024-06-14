using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productService;

        public ProductController(IProductRepository productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var product = await _productService.GetAllProductAsync();
            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            var createdProduct = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchInformation(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
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

            patchDoc.ApplyTo(product, ModelState);

            if (!TryValidateModel(product))
            {
                return ValidationProblem(ModelState);
            }

            var updatedInformation = await _productService.UpdateProductAsync(id, product);

            return Ok(updatedInformation);
        }



    }
}
