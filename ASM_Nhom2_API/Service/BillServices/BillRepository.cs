using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.BillServices
{
    public class BillRepository : IBillRepository
    {
        private readonly AppDbContext _context;

        public BillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Bill> CreateBillAsync(BillVM billVM)
        {
            var bill = new Bill
            {
                UserID = billVM.UserID,
                Quantity = billVM.Quantity,
                TotalAmount = billVM.TotalAmount,
                Status = "Pending"
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return bill;
        }

        public async Task<Bill> GetBillByIdAsync(int id)
        {
            return await _context.Bills
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BillId == id);
        }

        public async Task<IEnumerable<Bill>> GetAllBillsAsync()
        {
            return await _context.Bills
                .Include(b => b.User)
                .OrderBy(b => b.BillId)
                .ToListAsync();
        }

        public async Task<Bill> UpdateBillAsync(int id, BillVM billVM)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return null;
            }

            bill.UserID = billVM.UserID;
            bill.Quantity = billVM.Quantity;
            bill.TotalAmount = billVM.TotalAmount;
            bill.Status = billVM.Status ;

            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();

            return bill;
        }

        public async Task<bool> DeleteBillAsync(int id)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return false;
            }
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}