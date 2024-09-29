using Fx.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fx.Infrastructure.Data;

public class AppDbContext : DbContext
{
    // 订单表
    public DbSet<Order> Orders { get; set; }

    // 配置数据库上下文
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // 模型创建时的配置
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // 这里可以配置实体类映射
    }
}