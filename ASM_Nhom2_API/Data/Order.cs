using System.ComponentModel.DataAnnotations;
using System;

namespace ASM_Nhom2_API.Data
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }

        public float TotalAmount { get; set; }

        public virtual User User { get; set; }
    }
}