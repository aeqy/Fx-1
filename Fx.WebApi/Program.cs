using Fx.Application.Interfaces;
using Fx.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// 注册订单服务到DI容器中
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 配置HTTP请求管道
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
