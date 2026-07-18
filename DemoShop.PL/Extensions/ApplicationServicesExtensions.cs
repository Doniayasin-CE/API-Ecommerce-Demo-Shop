using DemoShop.BLL.Service;
using DemoShop.DAL.Repository;
using DemoShop.DAL.Utilities;
using Stripe;

namespace DemoShop.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
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
            services.AddScoped<IProductService, BLL.Service.ProductService>();
            services.AddScoped<IFileService, BLL.Service.FileService>();
            //Register the Brand Services
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();
            //Register the Cart Services
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();

            //Register the Checkout Services
            services.AddScoped<ICheckoutService, BLL.Service.CheckoutService>();
            //Register the Order Services
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            //Register the User Management Services
            services.AddScoped<IUserManagementService, UserManagementService>();

            //Register the Review Services
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewService, BLL.Service.ReviewService>();

            // Configure Stripe settings
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            return services;
        }
    }
}
