using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.ProductServices
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product info);
        Task<Product> UpdateProductAsync(int id, Product info);
        Task<bool> DeleteProductAsync(int id);
    }

}
