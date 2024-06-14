namespace ASM_Nhom2_View.Data
{
    public class BillDetails
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
    }
}
