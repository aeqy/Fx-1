using Fx.Domain.Entities;

namespace Fx.Domain.Repositories;

/// <summary>
/// 定义一个用户仓储接口，它只定义基本的数据操作，而不涉及数据库具体实现
/// </summary>
public interface IUserRepository
{
    // 获取用户
    Task<User> GetUserByUsernameAsync(string username);
    // 保存用户
    Task SaveUserAsync(User user);
}