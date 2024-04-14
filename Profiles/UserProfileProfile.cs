using AutoMapper;
using LoginSystem.Models;
using LoginSystem.Models.Dtos.UserProfileDtos;

namespace LoginSystem.Profiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<AddUserProfileDto, UserProfile>();
            CreateMap<UpdateUserProfileDto, UserProfile>();
            CreateMap<UserProfile, ReadUserProfileDto>();
        }
    }
}