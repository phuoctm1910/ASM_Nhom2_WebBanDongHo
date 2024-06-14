namespace ASM_Nhom2_View.Models
{
    public class BillVM
    {
        public int BillId { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }
        public float TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
