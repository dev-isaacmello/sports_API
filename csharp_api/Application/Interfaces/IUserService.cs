using csharp_api.Domain.Entities;

namespace csharp_api.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<Users>> GetAllUsersAsync();
    
    Task<Users> GetUserByIdAsync(int id);
    
    Task<Users> InserUserAsync(Users user);
    
    Task<Users> UpdateUserAsync(int id, Users user);
    
    Task DeleteAsync(int id);
}