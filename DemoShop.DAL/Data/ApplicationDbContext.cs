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
