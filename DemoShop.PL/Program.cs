using DemoShop.BLL.Mapping;
using DemoShop.DAL.Utilities;
using DemoShop.PL.Extensions;
using DemoShop.PL.Middlewares;
using Microsoft.Extensions.Options;

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

            // add CORS Policies services 
            builder.Services.AddCorsPolicyServices();

            // add AddDbContext services 
            builder.Services.AddDatabaseServices(builder.Configuration);

            // add AddLocalization services
            builder.Services.AddLocalizationServices();

            // Register the Application Services
            builder.Services.AddApplicationServices(builder.Configuration);

            // 1.Register Identity and its core managers 2.Tell Identity to use our EF Core DbContext
            builder.Services.AddIdentityServices();

            // register Authentication and its configration
            builder.Services.AddJwtAuthenticationServices(builder.Configuration);

            // Register the AddHttpContextAccessor Service
            builder.Services.AddHttpContextAccessor();

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
            app.UseCors(CorsPolicyExtensions.PolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            //Global Exception Handler modern .net 8+ 
            //*app.UseExceptionHandler();

            //Inline Custom Middleware
            app.Use(async(context,next) =>
            {
                Console.WriteLine("Processing Request...");
                await next();
                Console.WriteLine("Processing Response...");
            });
            //Use Custom Middleware class
            //*app.UseMiddleware<CustomMiddleware>();
            //Use Custom Middleware Extentsion method
            //*app.UseCustomMiddleware();
            //Use Custom Global Exception Handler
            app.UseGlobalExceptionHandler();

            app.MapControllers();
            app.Run();
        }
    }
}
