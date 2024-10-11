using Fx.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fx.Infrastructure.Data;

/// <summary>
/// IdentityDbContext 泛型类，用于处理 Identity 用户和角色
/// </summary>
// public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
public class AppDbContext : IdentityDbContext<User>
{
    
    // 订单表
    public DbSet<Order> Orders { get; set; }
    
    // 如果没有包含 IdentityUser 的 DbSet，添加以下代码
    public DbSet<User> Users { get; set; }

    // 配置数据库上下文
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // 模型创建时的配置
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // 这里可以配置实体类映射
        
        modelBuilder.UseOpenIddict(); // 使用 OpenIddict
    }
}