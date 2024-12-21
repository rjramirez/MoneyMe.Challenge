using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiConfiguration
{
    public static class ApiServices
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Api Versioning
            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = false;
                cfg.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader(ApiVersionConstant.HeaderKey),
                    new QueryStringApiVersionReader(ApiVersionConstant.QueryString));
            });

            //Api Versioned Explorer
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            //Api Versioned Documentation
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ApiDocumentation>();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition(ApiDocumentConstant.Bearer, new OpenApiSecurityScheme
                {
                    Name = ApiDocumentConstant.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = ApiDocumentConstant.Description,
                    Scheme = ApiDocumentConstant.Bearer
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiDocumentConstant.Bearer
                            },

                            Scheme = ApiDocumentConstant.OAuth2,
                            Name = ApiDocumentConstant.Bearer,
                            In = ParameterLocation.Header
                        }, new List<string>()
                    }
                });
            });
        }
    }

    internal class ApiDocumentation : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public ApiDocumentation(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"MoneyMeAPI V{ description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
            }
        }
    }
}
