using csharp_api.Domain.Entities;

namespace csharp_api.Domain.Interfaces;

// Contrato com o repositório
public interface IUserRepository
{
    Task<IEnumerable<Users>> GetAllUsersAsync();
    Task<Users> GetUserByIdAsync(int id);
    Task<Users> InsertUserAsync(Users user);
    Task<Users> UpdateUserAsync(int id, Users user);
    Task DeleteAsync(int id);

}