namespace ASM_Nhom2_API.Data
{
    public class Bill
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public float Sale { get; set; }
        public float Total { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }

}
