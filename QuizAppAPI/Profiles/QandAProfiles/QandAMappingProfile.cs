using AutoMapper;
using QuizAppAPI.Model.DTO.QandAs;
using QuizAppAPI.Model.DTO.QandAsDTOs;
using QuizAppAPI.Model.DTO.Quiz;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Profiles.QandAProfiles
{
    public class QandAMappingProfile : Profile
    {
        public QandAMappingProfile()
        {
            CreateMap<QandA, QandAInfoDTO>();
            CreateMap<QandA, AddUpQandADTO>();
        }
    }
}
