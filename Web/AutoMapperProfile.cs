using System.Collections.Generic;
using Application.Dto;
using Application.Entities;
using AutoMapper;

namespace Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Question, QuestionDto>().ForMember(dto => dto.Answers,
                opt => opt.MapFrom<AnswersResolver>())
                .ReverseMap()
                .ForMember(question => question.Answers, opt => opt.MapFrom<NewAnswersResolver>());

            // CreateMap<Question, QuestionDto>().ForMember(dto => dto.Answers,
            //     expression => expression.MapFrom(question => question.Answers.Split(' ').ToList()));
        }
    }

    public class AnswersResolver : IValueResolver<Question, QuestionDto, ICollection<string>>
    {
        public ICollection<string> Resolve(Question source, QuestionDto destination, ICollection<string> destMember, ResolutionContext context)
        {
            return source.Answers.Split(' ');
        }
    }

    public class NewAnswersResolver : IValueResolver<QuestionDto, Question, string>
    {
        public ICollection<string> Resolve(Question source, QuestionDto destination, ICollection<string> destMember, ResolutionContext context)
        {
            return source.Answers.Split(' ');
        }


        public string Resolve(QuestionDto source, Question destination, string destMember, ResolutionContext context)
        {
            return string.Join(' ', source.Answers);
        }
    }
}