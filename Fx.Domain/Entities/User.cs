using Microsoft.AspNetCore.Identity;

namespace Fx.Domain.Entities;

public class User: IdentityUser
{
    public string Id { get; set; }       // 用户ID
    public string Username { get; set; } // 用户名
    public string Email { get; set; }    // 邮箱
    public string PasswordHash { get; set; }  // 存储加密后的密码
    public DateTime CreatedAt { get; set; }   // 用户创建时间
    
    // 领域行为：例如，设置密码时进行加密
    public void SetPassword(string password, IPasswordHasher passwordHasher)
    {
        PasswordHash = passwordHasher.HashPassword(password);
    }

    // 领域行为：检查密码是否正确
    public bool ValidatePassword(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.VerifyHashedPassword(PasswordHash, password);
    }
}
// 密码加密接口
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}