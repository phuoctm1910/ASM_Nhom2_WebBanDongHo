using System.Collections.Generic;

namespace ASM_Nhom2_API.Data
{
    public class Bill
    {
        public int BillId { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }
        public float TotalAmount { get; set; }
        public string Status { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BillDetails> BillDetails { get; set; } = new HashSet<BillDetails>();
    }

}
