using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace Fx.Infrastructure.Data;

/// <summary>
/// 种子数据
/// </summary>
public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        // 创建用户和角色
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 添加种子用户和角色
        await EnsureRolesAsync(roleManager);
        await EnsureUsersAsync(userManager);

        // 添加 OpenIddict 客户端种子数据
        var applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        await EnsureOpenIddictApplicationsAsync(applicationManager);
    }

    // 创建默认角色
    private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
    
    // 创建默认用户
    private static async Task EnsureUsersAsync(UserManager<IdentityUser> userManager)
    {
        var adminUser = new IdentityUser { UserName = "admin", Email = "a@eqy.cc", EmailConfirmed = true };
        if (await userManager.FindByNameAsync(adminUser.UserName) == null)
        {
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    
    // 创建 OpenIddict 客户端
    private static async Task EnsureOpenIddictApplicationsAsync(IOpenIddictApplicationManager applicationManager)
    {
        // 检查客户端是否存在
        if (await applicationManager.FindByClientIdAsync("your-client-id") == null)
        {
            await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "your-client-id",
                ClientSecret = "your-client-secret",
                DisplayName = "My Client Application",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles
                }
            });
        }
    }
    
    
}