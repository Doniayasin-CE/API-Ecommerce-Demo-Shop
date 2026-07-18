using DemoShop.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DemoShop.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoriesTranslations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandTranslation> BrandTranslations { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Orderltem> Orderltems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
           IHttpContextAccessor HttpContextAccessor) 
            : base(options)
        {
            _httpContextAccessor = HttpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            // Category -> User Relationship
            builder.Entity<Category>()
                .HasOne(r=>r.CreatedBy)
                .WithMany()
                .HasForeignKey(fk=>fk.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Category>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(fk => fk.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Product -> User Relationship
            builder.Entity<Product>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(fk => fk.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Product>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(fk => fk.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
            // Product -> Category Relationship
            builder.Entity<Product>()
                .HasOne(p => p.Category) //Product Relation side
                .WithMany(c => c.Products) //Category Relation side
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // Product -> Brand Relationship
            builder.Entity<Product>()
               .HasOne(p => p.Brand) //Product Relation side
               .WithMany(c => c.Products) //Brand Relation side
               .HasForeignKey(p => p.BrandId)
               .OnDelete(DeleteBehavior.Restrict);
            // Brand -> User Relationship
            builder.Entity<Brand>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(fk => fk.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Brand>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(fk => fk.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if(_httpContextAccessor.HttpContext != null)
            {
                var entries = ChangeTracker.Entries<AuditableEntity>();
                var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != null)
                {
                    foreach (var entry in entries)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                            entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                        }

                        if (entry.State == EntityState.Modified)
                        {
                            entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                            entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                        }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
