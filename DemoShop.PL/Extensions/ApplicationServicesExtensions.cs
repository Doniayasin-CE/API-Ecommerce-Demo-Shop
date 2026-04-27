using DemoShop.BLL.Service;
using DemoShop.DAL.Repository;
using DemoShop.DAL.Utilities;

namespace DemoShop.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register the ICategoryRepository 
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            // Register the ICategoryService 
            services.AddScoped<ICategoryService, CategoryService>();
            //Register our custom Authentication Service
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            //Register our custom RoleSeedData Service
            services.AddScoped<ISeedData, RoleSeedData>();
            // Register the IEmailSender Service
            services.AddTransient<IEmailSender, EmailSender>();
            
            //Register the Product Services
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFileService, FileService>();
            //Register the Brand Services
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();
            //Register the Cart Services
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();

            return services;
        }
    }
}
