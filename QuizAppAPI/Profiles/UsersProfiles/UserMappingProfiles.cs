using AutoMapper;
using QuizAppAPI.Model.DTO.Users;
using QuizAppAPI.Model.DTO.UsersDTOs;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Profiles.UsersProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles() 
        {
            CreateMap<User, UserInfoDTO>();
            CreateMap<User, UserLoginRegDTO>();
            CreateMap<User, UserChangePassDTO>();
            CreateMap<User, LoginResponseDTO>();
            CreateMap<User, UserRegisterDTO>();

            CreateMap<User, UserInfoDTO>()
                .ForMember(dest => dest.QuizInfoDTOs, opt => opt.MapFrom(src => src.Quizzes));

        }
    }
}
