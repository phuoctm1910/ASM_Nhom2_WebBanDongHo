
namespace ASM_Nhom2_API.Model
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductStock { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
