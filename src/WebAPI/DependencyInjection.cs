using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.WebAPI.Filters;
using CleanArchitecture.WebAPI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CleanArchitecture.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>());
                // .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        services.AddSwaggerGen(options =>
        {
            // 添加一个安全方案
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });
            // 使用Swashbuckle.AspNetCore.Filters包中SecurityRequirementsOperationFilter为所有注有[Authorize]特性的Action添加安全方案
            options.OperationFilter<SecurityRequirementsOperationFilter>(true, JwtBearerDefaults.AuthenticationScheme);// OperationFilter<T>(params object[] arguments)的参数会传递给T类型的构造函数，SecurityRequirementsOperationFilter的第二个参数必须与添加的安全方案的名称相同
        });

        // services.AddOpenApiDocument(configure =>
        // {
        //     configure.Title = "CleanArchitecture API";
        //     configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        //     {
        //         Type = OpenApiSecuritySchemeType.ApiKey,
        //         Name = "Authorization",
        //         In = OpenApiSecurityApiKeyLocation.Header,
        //         Description = "Type into the textbox: Bearer {your JWT token}."
        //     });
        //
        //     configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        // });

        return services;
    }
}
