using csharp_api.Application.Interfaces;
using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;

namespace csharp_api.Application.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<IEnumerable<Users>> GetAllUsersAsync() => await repository.GetAllUsersAsync();
    
    public async Task<Users> GetUserByIdAsync(int id) => await repository.GetUserByIdAsync(id);
    
    public async Task<Users> InserUserAsync(Users user) => await repository.InsertUserAsync(user);
    
    public async Task<Users> UpdateUserAsync(int id, Users user) => await repository.UpdateUserAsync(id, user);
    
    public async Task DeleteAsync(int id) => await repository.DeleteAsync(id);
}