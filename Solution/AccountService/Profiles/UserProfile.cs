using AutoMapper;
using Entities;

namespace AccountService.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile() 
        {
            CreateMap<UserCreateDto, User>();
            CreateMap<User?, UserDto>();
            CreateMap<UserUpdateDto,User>();
            CreateMap<CardForUserCreationDto, Card>();
            CreateMap<CardForUserUpdateDto, Card>();


        }
    }
}
