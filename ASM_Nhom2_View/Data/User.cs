using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASM_Nhom2_View.Data
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public bool? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Bill> Bills { get; set; } = new HashSet<Bill>();
    }
}
