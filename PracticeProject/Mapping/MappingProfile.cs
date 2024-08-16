using AutoMapper;
using PracticeProject.DTOs;
using PracticeProject.Models;

namespace PracticeProject.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ClientModel, Client>();
            CreateMap<ClientModel, User>();
            CreateMap<Client, GetClientsDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        }

    }
}
