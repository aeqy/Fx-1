using Fx.Domain.Entities;
using Fx.Domain.Repositories;
using Fx.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fx.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetUserByUsernameAsync(string username)
    { 
        return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
    }
    
    
    public async Task SaveUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // 等待异步保存操作完成，无需返回值
    }
}