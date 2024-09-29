namespace Fx.Domain.Entities;

// 订单实体类，表示订单的基本信息
public class Order
{
    public int Id { get; set; } // 订单ID
    public string ProductName { get; set; } // 产品名称
    public decimal Price { get; set; } // 价格
}