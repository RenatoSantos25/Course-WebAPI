using ReserveiAPI.Objects.Models.Entities;

namespace ReserveiAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<UserModel> GetByID(int id);
        Task<UserModel> Create(UserModel model);

        Task<UserModel> Update(UserModel model);
        Task<UserModel> Delete(UserModel model);

    }
}
