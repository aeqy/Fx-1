using Fx.Application.Interfaces;
using Fx.Application.Services;
using Fx.Infrastructure;
using Fx.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(config =>
{
    config.AddDebug();
    config.AddConsole();
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug); // 设置为详细日志级别

// // 配置 PostgreSQL 数据库上下文
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// 添加应用服务, 注入数据库连接字符串
//builder.Services.AddAppServices(builder.Configuration.GetConnectionString("PostgresConnection"));


// 配置ASP.NET Identity, 使用EF Core存储
builder.Services.AddIdentity<IdentityUser, IdentityRole>() // 使用默认的实体类型
    .AddEntityFrameworkStores<AppDbContext>()              // 使用EF Core存储
    .AddDefaultTokenProviders();                           // 添加默认令牌提供程序


// 添加 OpenIddict, 配置授权服务器
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AppDbContext>(); // 使用 EF Core, 使用 AppDbContext
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetTokenEndpointUris("/connect/token");

        options.AllowPasswordFlow() // 允许密码模式
            .AllowRefreshTokenFlow(); // 允许刷新令牌模式

        options.AcceptAnonymousClients(); // 允许匿名客户端
        
        options.AddEphemeralEncryptionKey() // 使用临时加密密钥 (仅开发时使用)
            .AddEphemeralSigningKey(); // 使用临时签名密钥 (仅开发时使用)
        
        // 启用 Token 端点通过 ASP.NET Core 处理
        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough(); // 必须启用此选项
    })
    .AddValidation(options =>
    {
        options.UseLocalServer(); // 使用本地服务器
        options.UseAspNetCore(); // 使用 ASP.NET Core
    });


// 注册订单服务到DI容器中
builder.Services.AddScoped<IOrderService, OrderService>();
// 注册账户服务到DI容器中
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddControllers();

// 配置 Swagger, 添加 Swagger UI 
builder.Services.AddSwaggerGen((c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fx.WebApi", Version = "v1" });

    // 添加 JWT 安全性定义, 用于 Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // 添加 JWT 安全性要求, 用于 Swagger UI
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
            Array.Empty<string>()
        }
    });
}));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 在应用启动时调用 SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 调用 SeedData 来初始化数据库中的用户、角色和 OpenIddict 客户端
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding data: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseDeveloperExceptionPage(); // 开启详细错误信息
}

//app.UseHttpsRedirection();

app.UseDeveloperExceptionPage(); // 在开发环境中启用详细错误页面


app.UseAuthentication(); // 添加身份验证中间件

// 配置HTTP请求管道
app.UseHttpsRedirection();  // 添加HTTPS重定向
app.UseAuthorization(); // 添加授权中间件
app.MapControllers();





app.Run();
