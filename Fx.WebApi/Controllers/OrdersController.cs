using Fx.Application.Interfaces;
using Fx.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fx.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
// 订单控制器，处理订单相关的API请求
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    // 构造函数注入订单服务
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        // 获取所有订单
        var orders = await _orderService.GetOrders();
        return Ok(orders);
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        // 根据ID获取订单
        var order = await _orderService.GetOrderById(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] Order order)
    {
        // 添加新订单
        await _orderService.AddOrder(order);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }
}