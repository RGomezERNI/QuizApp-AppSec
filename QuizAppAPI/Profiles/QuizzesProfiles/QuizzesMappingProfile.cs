using AutoMapper;
using QuizAppAPI.Model.DTO.Quiz;
using QuizAppAPI.Model.DTO.QuizDTOs;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Profiles.QuizzesMappingProfile
{
    public class QuizzesMappingProfile : Profile
    {
        public QuizzesMappingProfile() 
        {
            CreateMap<Quiz, QuizInfoDTO>();
            CreateMap<Quiz, AddUpQuizDTO>().ReverseMap();
            CreateMap<Quiz, PatchQuizDTO>();
        }
        
    }
}
