using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.RoleServices
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRoleAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task AddRoleAsync(RoleVM roleVM);
        Task UpdateRoleAsync(int roleId, RoleVM roleVM);
        Task DeleteRoleAsync(int roleId);
    }
}
