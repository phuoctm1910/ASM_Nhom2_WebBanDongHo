﻿namespace ASM_Nhom2_View.Models
{
    public class BillDetailVM
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
