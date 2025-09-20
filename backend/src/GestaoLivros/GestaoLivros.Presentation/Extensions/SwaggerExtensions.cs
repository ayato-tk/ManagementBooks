using Microsoft.OpenApi.Models;

namespace GestaoLivros.Presentation.Extensions;

public static class SwaggerExtensions
{

    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestaoLivros", Version = "v1", Description = "Gestao Livros Teste Doc"});
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In =  ParameterLocation.Header,
                Description = "Por favor utilize Bearer <TOKEN>",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
            
        });
    }
    
}