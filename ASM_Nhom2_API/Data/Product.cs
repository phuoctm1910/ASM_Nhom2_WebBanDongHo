using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASM_Nhom2_API.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int ProductStock { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public virtual ICollection<Bill> Bills { get; set; } = new HashSet<Bill>();

    }
}
