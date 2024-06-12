using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASM_Nhom2_View.Data
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
