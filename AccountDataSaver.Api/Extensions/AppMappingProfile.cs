using AccountDataSaver.Api.Contracts;
using AccountDataSaver.Application.Contracts;
using AccountDataSaver.Application.Models;
using AccountDataSaver.Core.Models;
using AccountDataSaver.Presistence.Entities;
using AutoMapper;

namespace AccountDataSaver.Api.Extensions;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<UserEntity, UserModel>().ReverseMap();
        CreateMap<UserAccountEntity, UserAccountModel>().ReverseMap();

        CreateMap<AddAccountRequest, AddAccountRequestModel>().ReverseMap();
        CreateMap<GetAccountRequest, GetAccountRequestModel>().ReverseMap();
        CreateMap<AddAccountRequest, AddAccountRequestModel>().ReverseMap();

        CreateMap<LoginUserRequest, LoginUserRequestModel>().ReverseMap();
        CreateMap<RegisterUserRequest, RegisterUserRequestModel>().ReverseMap();
    }
}