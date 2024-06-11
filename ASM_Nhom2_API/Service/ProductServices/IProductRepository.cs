using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.ProductServices
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductVM>> GetAllProductsAsync();
        Task<ProductVM> GetProductByIdAsync(int productId);
        Task AddProductAsync(ProductVM productVM);
        Task UpdateProductAsync(int productId, ProductVM productVM);
        Task DeleteProductAsync(int productId);
    }

}
