using DemoShop.BLL.Mapping;
using DemoShop.BLL.Service;
using DemoShop.DAL.Data;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using DemoShop.DAL.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

namespace DemoShop.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //Enable CORS Policies Configuration
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,policy =>{
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // add AddDbContext services 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // add AddLocalization services
            builder.Services.AddLocalization(options => options.ResourcesPath="");
            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar")
            };
            builder.Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
                //options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                //{
                //    QueryStringKey = "lang"
                //});
            });

            // Register the ICategoryRepository 
            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
            // Register the ICategoryService 
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            //Register our custom Authentication Service
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            //Register our custom RoleSeedData Service
            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            // Register the IEmailSender Service
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            // Register the AddHttpContextAccessor Service
            builder.Services.AddHttpContextAccessor();
            //Register the Product Services
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IFileService, FileService>();
            //Register the Brand Services
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IBrandService, BrandService>();

            // 1.Register Identity and its core managers 2.Tell Identity to use our EF Core DbContext
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // register Authentication and its configration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                    };
                });
            MapsterConfig.RegisterMapsterConfiguration();
            var app = builder.Build();

            // use the AddLocalization services
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            // use IseedData and create all seedData classes
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();
                foreach(var seeder in seeders)
                {
                    await seeder.SeedDataAsync();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
