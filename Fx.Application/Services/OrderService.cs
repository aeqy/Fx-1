using Fx.Application.Interfaces;
using Fx.Domain.Entities;
using Fx.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fx.Application.Services;

// 订单服务实现类
public class OrderService : IOrderService
{
    // private readonly List<Order> _orders = new(); // 模拟订单数据的列表
    
    private  readonly AppDbContext _dbContext;
    
    // 构造函数
    public OrderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // 获取所有订单
    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await  _dbContext.Orders.ToListAsync(); // 从数据库中获取所有订单
    }

    // 根据ID获取单个订单
    public async Task<Order> GetOrderById(int id)
    {
        return await _dbContext.Orders.FindAsync(id); // 根据ID查找订单
    }

    // 添加新订单
    public async Task AddOrder(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(); // 保存到数据库
    }
}