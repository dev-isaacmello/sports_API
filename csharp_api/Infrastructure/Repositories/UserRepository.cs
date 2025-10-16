using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;
using csharp_api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace csharp_api.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    // método de busca de todos os usuários
    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        var users = await context.Users.ToListAsync() ?? throw new Exception("Nenhum usuário encontrado");
        return users;
    }
    
    // método que retorna apenas 1 usuário
    public async Task<Users> GetUserByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id) ?? throw new Exception("Usuário não encontrado");
        return user;
    }
    
    // método que retorna usuário por email
    public async Task<Users?> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    // método que vai adicionar um usuário
    public async Task<Users> InsertUserAsync(Users user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    // método que vai atualizar um usuário
    public async Task<Users> UpdateUserAsync(int id, Users user)
    {
        var existingUser = await context.Users.FindAsync(id) ?? throw new Exception("Usuário não encontrado");
        user.Id = id;
        context.Users.Entry(existingUser).CurrentValues.SetValues(user);
        await context.SaveChangesAsync();
        return existingUser;
    }
    
    // método que vai deletar um usuário
    public async Task DeleteAsync(int id)
    {
        var user = await context.Users.FindAsync(id) ?? throw new Exception("Usuário não encontrado");
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}