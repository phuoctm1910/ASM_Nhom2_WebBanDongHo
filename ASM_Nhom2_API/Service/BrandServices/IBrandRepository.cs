using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.BrandServices
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllBrandAsync();
        Task<Brand> GetBrandByIdAsync(int brandId);
        Task AddBrandAsync(BrandVM brandVM);
        Task UpdateBrandAsync(int brandId, BrandVM brandVM);
        Task DeleteBrandAsync(int brandId);
    }
}
