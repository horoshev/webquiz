using System.Collections.Generic;
using Application.Dto;
using Application.Entities;
using Application.Services;
using AutoMapper;

namespace Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(dto => dto.CorrectAnswers, opt => opt.MapFrom(question => Question.SplitAnswers(question.CorrectAnswers)))
                .ForMember(dto => dto.IncorrectAnswers, opt => opt.MapFrom(question => Question.SplitAnswers(question.IncorrectAnswers)))
            .ReverseMap()
                .ForMember(question => question.CorrectAnswers, opt => opt.MapFrom(dto => Question.JoinAnswers(dto.CorrectAnswers)))
                .ForMember(question => question.IncorrectAnswers, opt => opt.MapFrom(dto => Question.JoinAnswers(dto.IncorrectAnswers)));

            CreateMap<TriviaQuestion, Question>()
                .ForMember(question => question.AuthorId, member => member.MapFrom(question => "93e584ea-1846-43bc-9906-36beb0cff48c"))
                .ForMember(question => question.Text, member => member.MapFrom(question => question.Question))
                .ForMember(question => question.Category, member => member.MapFrom<CategoryResolver>())
                .ForMember(question => question.Difficulty, member => member.MapFrom(question => MapQuestionDifficulty(question.Difficulty)))
                .ForMember(question => question.CorrectAnswers, member => member.MapFrom(question => question.CorrectAnswer))
                .ForMember(question => question.IncorrectAnswers, member => member.MapFrom(question => Question.JoinAnswers(question.IncorrectAnswers)));

            CreateMap<Page<Question>, Page<QuestionDto>>()
                .ForMember(pageOut => pageOut.Data, member => member.MapFrom(pageIn => pageIn.Data));
        }

        private static QuestionDifficulty MapQuestionDifficulty(string questionType)
        {
            return questionType switch
            {
                "hard" => QuestionDifficulty.Hard,
                "medium" => QuestionDifficulty.Medium,
                "easy" => QuestionDifficulty.Easy,
                _ => QuestionDifficulty.None
            };
        }
    }

    public class CategoryResolver : IValueResolver<TriviaQuestion, Question, QuestionCategory>
    {
        public QuestionCategory Resolve(TriviaQuestion source, Question destination, QuestionCategory destMember, ResolutionContext context)
        {
            var map = new Dictionary<string, QuestionCategory>
            {
                ["General Knowledge"] = QuestionCategory.General,
                ["Entertainment: Books"] = QuestionCategory.Books,
                ["Entertainment: Film"] = QuestionCategory.Films,
                ["Entertainment: Music"] = QuestionCategory.Music,
                ["Entertainment: Musicals &amp; Theatres"] = QuestionCategory.None,
                ["Entertainment: Television"] = QuestionCategory.Animals,
                ["Entertainment: Video Games"] = QuestionCategory.VideoGames,
                ["Entertainment: Board Games"] = QuestionCategory.None,
                ["Science &amp; Nature"] = QuestionCategory.Nature,
                ["Science: Computers"] = QuestionCategory.Computers,
                ["Science: Mathematics"] = QuestionCategory.Mathematics,
                ["Mythology"] = QuestionCategory.Mythology,
                ["Sports"] = QuestionCategory.Sports,
                ["Geography"] = QuestionCategory.Geography,
                ["History"] = QuestionCategory.History,
                ["Politics"] = QuestionCategory.Politics,
                ["Art"] = QuestionCategory.Art,
                ["Celebrities"] = QuestionCategory.Animals,
                ["Animals"] = QuestionCategory.Animals,
                ["Vehicles"] = QuestionCategory.Vehicles,
                ["Entertainment: Comics"] = QuestionCategory.Comics,
                ["Science: Gadgets"] = QuestionCategory.Science,
                ["Entertainment: Japanese Anime &amp; Manga"] = QuestionCategory.Anime,
                ["Entertainment: Cartoon &amp; Animations"] = QuestionCategory.Cartoons
            };

            return map.TryGetValue(source.Category, out var category) ? category : QuestionCategory.None;
        }
    }
}