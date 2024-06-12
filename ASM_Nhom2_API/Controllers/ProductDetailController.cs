using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.ProductDetailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailRepository _productDetailService;

        public ProductDetailsController(IProductDetailRepository productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductDetails()
        {
            var productDetails = await _productDetailService.GetAllProductDetailsAsync();
            return Ok(productDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetailById(int id)
        {
            var productDetail = await _productDetailService.GetProductDetailByIdAsync(id);
            if (productDetail == null)
            {
                return NotFound();
            }
            return Ok(productDetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductDetail(ProductDetailVM productDetailVM)
        {
            var productDetail = await _productDetailService.AddProductDetailAsync(productDetailVM);
            return CreatedAtAction(nameof(GetProductDetailById), new { id = productDetail.ProductDetailID }, productDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductDetail(int id, ProductDetailVM productDetailVM)
        {
            var updatedProductDetail = await _productDetailService.UpdateProductDetailAsync(id, productDetailVM);
            if (updatedProductDetail == null)
            {
                return NotFound();
            }
            return Ok(updatedProductDetail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDetail(int id)
        {
            var result = await _productDetailService.DeleteProductDetailAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}