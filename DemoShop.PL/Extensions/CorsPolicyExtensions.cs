namespace DemoShop.PL.Extensions
{
    public static class CorsPolicyExtensions
    {
        public const string PolicyName = "_myAllowSpecificOrigins";
        public static IServiceCollection AddCorsPolicyServices(this IServiceCollection services)
        {
            //Enable CORS Policies Configuration
            services.AddCors(options =>
            {
                options.AddPolicy(name: PolicyName, policy => {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
