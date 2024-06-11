using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using ASM_Nhom2_API.Service.BillServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillRepository _billService;

        public BillsController(IBillRepository billService)
        {
            _billService = billService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills()
        {
            var bills = await _billService.GetAllBillsAsync();
            return Ok(bills);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bill>> GetBill(int id)
        {
            var bill = await _billService.GetBillByIdAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(bill);
        }

        [HttpPost]
        public async Task<ActionResult<Bill>> CreateBill(BillVM billVM)
        {
            var createdBill = await _billService.CreateBillAsync(billVM);
            return CreatedAtAction(nameof(GetBill), new { id = createdBill.Id }, createdBill);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(int id, BillVM billVM)
        {
            var updatedBill = await _billService.UpdateBillAsync(id, billVM);

            if (updatedBill == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var result = await _billService.DeleteBillAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
