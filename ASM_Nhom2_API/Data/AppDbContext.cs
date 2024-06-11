using Microsoft.EntityFrameworkCore;

namespace ASM_Nhom2_API.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=*;Initial Catalog=ASM_C#5;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(20);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasMany(p => p.Bills)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserID);
            });

            modelBuilder.Entity<Category>(entity => {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(50);

            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(p => p.Bills)
                .WithOne(b => b.Product)
                .HasForeignKey(b => b.ProductID);
            });
            modelBuilder.Entity<Brand>(entity => {
                entity.HasKey(entity => entity.BrandId);
            });


            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.ProductDetailID);
                


            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
