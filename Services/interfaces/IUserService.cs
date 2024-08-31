using ReserveiAPI.Objects.Contracts;
using ReserveiAPI.Objects.DTOs.Entities;
namespace ReserveiAPI.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTOs>> GetAll();
        Task<UserDTOs> GetByID(int id);
		Task<UserDTOs> GetByEmail(string email);
		Task<UserDTOs> Login(Login login);
		Task Create(UserDTOs userDTO);

        Task Update(UserDTOs userDTO);
        Task Delete(UserDTOs userDTO);
    }
}
