using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Identity.Models;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CleanArchitecture.Application.Tests.Integration;

public class CustomApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "CleanArchitectureTest", Username = "kitlau", Password = "password"
        }).Build();
    
    private IConfiguration _configuration = null!;
    private IServiceScopeFactory _scopeFactory = null!;
    private string? _currentUserId;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        _scopeFactory = this.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = this.Services.GetRequiredService<IConfiguration>();
        
        using (var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>())
        {
            if (dbContext.Database.IsNpgsql())
            {
                await dbContext.Database.MigrateAsync();
            }
        }
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("testing");
        
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureTestServices(services =>
        {
            services.Remove<ICurrentUserService>()
                .AddTransient(provider =>
                {
                    var currentUserService = Substitute.For<ICurrentUserService>();
                    currentUserService.UserId.Returns(GetCurrentUserId());
                    return currentUserService;
                });
                

            services.Remove<DbContextOptions<ApplicationDbContext>>()
                .Remove<ApplicationDbContext>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.UseNpgsql(_dbContainer.ConnectionString,
                        optionsBuilder =>
                            optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                });
        });
    }
    
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public string? GetCurrentUserId()
    {
        return _currentUserId;
    }

    public async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "test@local", "Testing1234!", Array.Empty<string>());
    }

    public async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", "administrator@local", "Administrator1234!",
            new[] { "Administrator" });
    }

    public async Task<string> RunAsUserAsync(string userName, string email, string password, string[] roles)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = userName, Email = email };

        var result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        if (result.Succeeded)
        {
            _currentUserId = user.Id;

            return _currentUserId;
        }

        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }
}