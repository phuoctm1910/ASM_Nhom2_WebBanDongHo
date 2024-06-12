using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.ProductDetailServices
{
    public interface IProductDetailRepository
    {
        Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync();
        Task<ProductDetail> GetProductDetailByIdAsync(int id);
        Task<ProductDetail> AddProductDetailAsync(ProductDetailVM productDetailVM);
        Task<ProductDetail> UpdateProductDetailAsync(int id, ProductDetailVM productDetailVM);
        Task<bool> DeleteProductDetailAsync(int id);
    }
}
