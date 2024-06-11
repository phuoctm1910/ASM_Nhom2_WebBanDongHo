using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.BillServices
{
    public interface IBillRepository
    {

        Task<Bill> CreateBillAsync(BillVM billVM);
        Task<Bill> GetBillByIdAsync(int id);
        Task<IEnumerable<Bill>> GetAllBillsAsync();
        Task<Bill> UpdateBillAsync(int id, BillVM billVM);
        Task<bool> DeleteBillAsync(int id);

    }
}
