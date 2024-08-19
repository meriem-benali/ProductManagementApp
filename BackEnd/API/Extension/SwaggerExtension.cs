using Microsoft.OpenApi.Models;

namespace API.Extension
{
    public static class SwaggerExtension
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product.Api",
                    Version = "v1",
                    Description = "Product Web Api ",
                });
            });
        }
    }
}
