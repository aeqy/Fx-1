using Fx.Application.Interfaces;
using Fx.Domain.Entities;

namespace Fx.Application.Services;

// 订单服务实现类
public class OrderService : IOrderService
{
    private readonly List<Order> _orders = new(); // 模拟订单数据的列表

    // 获取所有订单
    public Task<IEnumerable<Order>> GetOrders() => Task.FromResult(_orders.AsEnumerable());

    // 根据ID获取单个订单
    public Task<Order> GetOrderById(int id) =>
        Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

    // 添加新订单
    public Task AddOrder(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }
}