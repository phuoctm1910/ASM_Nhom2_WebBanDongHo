﻿using System.Collections.Generic;

namespace ASM_Nhom2_API.Data
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public virtual ICollection<ProductDetail> ProductDetail { get; set; } = new HashSet<ProductDetail>();
    }
}