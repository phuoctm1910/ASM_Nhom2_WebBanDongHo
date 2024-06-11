using System.Drawing;

namespace ASM_Nhom2_API.Data
{
    public class ProductDetail
    {
        public int ProductDetailID { get; set;}
        public string SubstanceGlass {  get; set;}
        public string MachineryWatch { get; set;}
        public int Diameter { get; set;}
        public int CaseSize { get; set;}
        public int Insurrance { get; set;}
        public string Origin { get; set;}
        public int BrandId { get; set; }
        public int ProductId { get; set;}
        public virtual Product Product { get; set;}
        public virtual Brand Brand { get; set;}

    }
}
