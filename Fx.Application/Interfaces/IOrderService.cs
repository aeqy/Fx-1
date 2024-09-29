using Fx.Domain.Entities;

namespace Fx.Application.Interfaces;

// 订单服务接口，定义获取和添加订单的操作
public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrders(); // 获取所有订单
    Task<Order> GetOrderById(int id); // 通过ID获取订单
    Task AddOrder(Order order); // 添加新订单
}