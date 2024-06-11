using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;

namespace ASM_Nhom2_API.Service.RoleServices
{

    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int categoryId)
        {
            return await _context.Roles.FindAsync(categoryId);
        }

        public async Task AddRoleAsync(RoleVM categoryVM)
        {
            var category = new Role
            {
                RoleName = categoryVM.RoleName
            };

            await _context.Roles.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(int categoryId, RoleVM categoryVM)
        {
            var category = await _context.Roles.FindAsync(categoryId);
            if (category != null)
            {
                category.RoleName = categoryVM.RoleName;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRoleAsync(int categoryId)
        {
            var category = await _context.Roles.FindAsync(categoryId);
            if (category != null)
            {
                _context.Roles.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}

