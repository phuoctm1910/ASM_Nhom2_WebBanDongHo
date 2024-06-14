using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_Nhom2_API.Model
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductStock { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImages { get; set; }
        public string Origin { get; set; }
        public string MachineType { get; set; }
        public int Diameter { get; set; }
        public string ClockType { get; set; }
        public int Insurrance { get; set; }
        public string Color { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        [NotMapped]
        public List<string> ProductImageList
        {
            get
            {
                if (string.IsNullOrEmpty(ProductImages))
                {
                    return new List<string>();
                }

                var cleanedProductImages = ProductImages.Replace("\\\"", "\"").Trim('\"');
                return JsonConvert.DeserializeObject<List<string>>(cleanedProductImages);
            }
            set
            {
                ProductImages = JsonConvert.SerializeObject(value);
            }
        }
    }
}
