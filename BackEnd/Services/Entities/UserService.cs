using AutoMapper;
using ReserveiAPI.Objects.Contracts;
using ReserveiAPI.Objects.DTOs.Entities;
using ReserveiAPI.Objects.Models.Entities;
using ReserveiAPI.Repositories.Interfaces;
using ReserveiAPI.Services.interfaces;
namespace ReserveiAPI.Services.Entities
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDTOs>> GetAll()
        {
            var userModel = await _userRepository.GetAll();
            userModel.ToList().ForEach(u => u.PasswordUser = "");
            return _mapper.Map<IEnumerable<UserDTOs>>(userModel);
        }
        public async Task<UserDTOs> GetByID(int id)
        {
            var userModel = await _userRepository.GetByID(id);
            if (userModel is not null) userModel.PasswordUser = "";
            return _mapper.Map<UserDTOs>(userModel);
        }
		public async Task<UserDTOs> GetByEmail(string email)
		{
			var userModel = await _userRepository.GetByEmail(email);
			if (userModel is not null) userModel.PasswordUser = "";
			return _mapper.Map<UserDTOs>(userModel);
		}
		public async Task<UserDTOs> Login(Login login)
		{
			var userModel = await _userRepository.Login(login);
			if (userModel is not null) userModel.PasswordUser = "";
			return _mapper.Map<UserDTOs>(userModel);
		}
		public async Task Create(UserDTOs userDTO)
        {
            var userModel = _mapper.Map<UserModel>(userDTO);
            await _userRepository.Create(userModel);

            userDTO.Id = userModel.Id;
            userDTO.PasswordUser = "";
        }
        public async Task Update(UserDTOs userDTO)
        {
            var userModel = _mapper.Map<UserModel>(userDTO);
            await _userRepository.Update(userModel);

            userDTO.PasswordUser = "";
        }
        public async Task Delete(UserDTOs userDTO)
        {
            var userModel = _mapper.Map<UserModel>(userDTO);
            await _userRepository.Delete(userModel);


            userDTO.PasswordUser = "";
        }
    }
}
