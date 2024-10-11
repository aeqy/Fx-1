using System.Security.Claims;
using Fx.Application.DTOs;
using Fx.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;


namespace Fx.Application.Services;

/// <summary>
/// 账户服务
/// </summary>
public class AccountService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IOpenIddictTokenManager tokenManager)
    : IAccountService
{
    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IdentityResult> RegisterAsync(RegisterDto model)
    {
        var user = new  IdentityUser()
        {
            UserName = model.UserName,
            Email = model.Email,
           // PasswordHash = model.Password
        };
        
        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return result;
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// 用户登陆并生成token,jwt 令牌
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> LoginAsync(LoginDto model)
    {
        var user = await  userManager.FindByNameAsync(model.UserName);
        if (user ==null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new InvalidCastException("用户名或密码错误");
        }
        
        // 生成 OpenIddict 访问令牌
        var principal = await signInManager.CreateUserPrincipalAsync(user);
        
        // 添加必要的声明
        principal.SetScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile, Scopes.OfflineAccess);
        principal.SetResources("resource_server");

        var token = await GenerateTokenAsync(principal);
        return token;
    }

    /// <summary>
    /// 生成JWT 令牌
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<string> GenerateTokenAsync(ClaimsPrincipal principal)
    {
        // 创建访问令牌
        var tokenDescriptor = new OpenIddictTokenDescriptor
        {
            Principal = principal, // 设置令牌的主体部分
            Subject = principal.FindFirst(Claims.Subject)?.Value, // 设置令牌的主体部分
            Type = TokenTypes.Bearer,   // 令牌类型
            //Type = TokenUsages.AccessToken, // 令牌类型
            //Lifetime = TimeSpan.FromHours(1) ,// 令牌有效期,默认为 1 小时
           
            ExpirationDate = DateTimeOffset.UtcNow.AddHours(1) // 1小时后过期
        };

        // 调用 OpenIddict API 来创建访问令牌
        var token = await tokenManager.CreateAsync(tokenDescriptor, CancellationToken.None);

        return token.ToString() ?? string.Empty; // 返回令牌字符串, 如果为空则返回空字符串
    }
}