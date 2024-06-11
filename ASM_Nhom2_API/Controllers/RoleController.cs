using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.RoleServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleService;
        private readonly AppDbContext _context;
        public RoleController(IRoleRepository roleService, AppDbContext context)
        {
            _roleService = roleService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var categories = await _roleService.GetAllRoleAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var category = await _roleService.GetRoleByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] RoleVM categoryVM)
        {
            await _roleService.AddRoleAsync(categoryVM);
            var createdRole = await _context.Roles.FirstOrDefaultAsync(c => c.RoleName == categoryVM.RoleName);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.RoleId }, createdRole);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleVM categoryVM)
        {
            var category = await _roleService.GetRoleByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _roleService.UpdateRoleAsync(id, categoryVM);

            var updatedRoles = await _roleService.GetRoleByIdAsync(id);
            return Ok(updatedRoles);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var category = await _roleService.GetRoleByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _roleService.DeleteRoleAsync(id);

            var afterdeletedRoles = await _roleService.GetAllRoleAsync();
            return Ok(afterdeletedRoles);
        }
    }
}
