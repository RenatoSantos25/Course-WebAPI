using AutoMapper;
using ReserveiAPI.Objects.DTOs.Entities;
using ReserveiAPI.Objects.Models.Entities;

namespace ReserveiAPI.Objects.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
        
            CreateMap<UserDTOs, UserModel>().ReverseMap();
        
        }
    }
}
