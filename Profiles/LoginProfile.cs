using AutoMapper;
using LoginSystem.Models;
using LoginSystem.Models.Dtos.LoginDtos;

namespace LoginSystem.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<AddLoginDto, Login>();
            CreateMap<UpdateEmailDto, Login>();
            CreateMap<UpdatePasswordDto, Login>();
            CreateMap<Login, ReadLoginDto>();
        }
    }
}