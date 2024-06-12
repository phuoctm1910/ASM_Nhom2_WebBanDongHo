using ASM_Nhom2_API.Data;
using ASM_Nhom2_API.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM_Nhom2_API.Service.ProductDetailServices
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly AppDbContext _context;

        public ProductDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync()
        {
            return await _context.ProductDetails.Include(pd => pd.Product).Include(pd => pd.Brand).ToListAsync();
        }

        public async Task<ProductDetail> GetProductDetailByIdAsync(int id)
        {
            return await _context.ProductDetails.Include(pd => pd.Product).Include(pd => pd.Brand).FirstOrDefaultAsync(pd => pd.ProductDetailID == id);
        }

        public async Task<ProductDetail> AddProductDetailAsync(ProductDetailVM productDetailVM)
        {
            var productDetail = new ProductDetail
            {
                SubstanceGlass = productDetailVM.SubstanceGlass,
                MachineryWatch = productDetailVM.MachineryWatch,
                Diameter = productDetailVM.Diameter,
                CaseSize = productDetailVM.CaseSize,
                Insurrance = productDetailVM.Insurrance,
                Origin = productDetailVM.Origin,
                BrandId = productDetailVM.BrandId,
                ProductId = productDetailVM.ProductId
            };

            _context.ProductDetails.Add(productDetail);
            await _context.SaveChangesAsync();

            return productDetail;
        }

        public async Task<ProductDetail> UpdateProductDetailAsync(int id, ProductDetailVM productDetailVM)
        {
            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return null;
            }

            productDetail.SubstanceGlass = productDetailVM.SubstanceGlass;
            productDetail.MachineryWatch = productDetailVM.MachineryWatch;
            productDetail.Diameter = productDetailVM.Diameter;
            productDetail.CaseSize = productDetailVM.CaseSize;
            productDetail.Insurrance = productDetailVM.Insurrance;
            productDetail.Origin = productDetailVM.Origin;
            productDetail.BrandId = productDetailVM.BrandId;
            productDetail.ProductId = productDetailVM.ProductId;

            _context.ProductDetails.Update(productDetail);
            await _context.SaveChangesAsync();

            return productDetail;
        }

        public async Task<bool> DeleteProductDetailAsync(int id)
        {
            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return false;
            }

            _context.ProductDetails.Remove(productDetail);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}