using Fx.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fx.Infrastructure;

/// <summary>
/// 这个类用来注册所有服务, 包括框架服务，业务服务，第三方服务
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// 静态方法, 用来注册基础设施层服务
    /// </summary>
    /// <param name="services">依赖注入容器 IServiceCollection</param>
    /// <param name="connectionString">用于数据库连接字符串</param>
    /// <returns>返回已经配置了基础设施服务的 IServiceCollection</returns>
    public static IServiceCollection AddAppServices(this IServiceCollection services, string? connectionString)
    {
        // 注册数据库上下文
        services.AddDbContext<AppDbContext>(options => { 
            // 使用 PostgreSQL, 并使用指定的连接字符串
            options.UseNpgsql(connectionString);
            // 使用 OpenIddict, 用于实现 OAuth2.0
            options.UseOpenIddict();
            
        });

        // 注册 Identity, 用于用户认证, 角色等
        services.AddIdentity<IdentityUser, IdentityRole>()
            // 使用数据库存储
            .AddEntityFrameworkStores<AppDbContext>()
            // 使用默认的令牌提供者
            .AddDefaultTokenProviders();

        // 注册 OpenIddict, 用于实现 OAuth2.0
        services.AddOpenIddict()
            // 配置 OpenIddict 的核心服务, 使用 EntityFrameworkCore 作为存储
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<AppDbContext>();
            })
            
            // 配置 OpenIddict 的服务器服务,·径为 /api/auth/token
            .AddServer(options =>
            {
                options.AllowAuthorizationCodeFlow()    // 允许授权码模式, 用于获取访问令牌
                    .RequireProofKeyForCodeExchange();  // 要求 PKCE, 用于保护令牌

                // 配置端点
                options.SetAuthorizationEndpointUris("api/auth/authorize")
                    .SetTokenEndpointUris("api/auth/token")
                    .SetUserinfoEndpointUris("api/auth/userinfo");

                // 允许授权码模式
                options.AllowAuthorizationCodeFlow();
                
            })
            
            // 配置 OpenIddict 的令牌验证服务
            .AddValidation(options =>
            {
                options.UseLocalServer();   // 使用本地服务器
                options.UseAspNetCore();    // 使用 ASP.NET Core
            });
        
        // 返回配置完毕的服务集合
        return  services;
    }

}