using Fx.Application.DTOs;
using Fx.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fx.WebApi.Controllers;

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    
    /// <summary>
    /// 注册新用户
    /// </summary>
    /// <param name="model">注册信息,包括用户名,邮箱,密码等</param>
    /// <returns>如果注册成功,返回200 OK; 否则返回 400 Bad Request</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        // 调用 AccountService 的注册方法处理逻辑
        var result = await _accountService.RegisterAsync(model);
        
        // 如果注册失败,返回错误信息
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    
    /// <summary>
    /// 用户登陆
    /// </summary>
    /// <param name="model">登陆信息和密码</param>
    /// <returns>如果成功200 OK 返回包含JWT 令牌; 否则401 Unauthorized</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        // 调用 AccountService 的登陆方法处理逻辑
        var result = await _accountService.LoginAsync(model);
        
        return Ok(new { Token = result }); // 返回包含 JWT 令牌的对象
    }
    
    [Route("connect/test")]
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Test route is working.");
    }
    
//     [Route("connect/token")]
    //     [HttpPost]
    //     public IActionResult TokenTest()
    //     {
    //         return Ok("Token endpoint test successful.");
    //     }
     }