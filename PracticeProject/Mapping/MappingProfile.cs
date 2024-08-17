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
            CreateMap<CreateTransactionDTO, Transaction>().ReverseMap();
            CreateMap<CreatePaymentMethodDTO, PaymentMethod>().ReverseMap();
            CreateMap<PaymentMethod, GetPaymentMethodsDTO>();
            CreateMap<Transaction, GetTransactionsDTO>()
                .ForMember(dest => dest.Clients, opt => opt.MapFrom(src =>
                    new List<GetClientsDTO> { new GetClientsDTO
                    {
                        UserId = src.Client.User.Id,
                        UserName = src.Client.User.UserName,
                        Email = src.Client.User.Email
                    }}))
                .ForMember(dest => dest.PaymentMethods, opt => opt.MapFrom(src =>
                    new List<GetPaymentMethodsDTO> { new GetPaymentMethodsDTO
                    {
                        PaymentMethodId = src.PaymentMethod.PaymentMethodId,
                        Name = src.PaymentMethod.Name,
                        AvailableBalance = src.PaymentMethod.AvailableBalance
                    }}));

            CreateMap<Transaction, GetTransactionDTO>();
            CreateMap<UpdateTransactionDTO, Transaction>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<PaymentMethod, PaymentMethodDTO>().ReverseMap();
        }

    }
}
