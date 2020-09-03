using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Entities;
using Application.Interfaces;
using AutoMapper;

namespace Application.Services
{
    public class QuestionService : BaseService<QuestionDto, Question>, IQuestionService
    {
        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public Question? GetRandomQuestion()
        {
            var count = Repository.Count();
            var index = new Random().Next(count);

            return Repository.GetByIndex(index);
        }

        public IEnumerable<Question> GetQuestionsByQuery(QuestionQuery query)
        {
            query ??= new QuestionQuery();

            return Repository.Query(query.Expression);
        }

        public IEnumerable<Question> GetQuestionByAuthorId(string authorId)
        {
            if (authorId is null)
            {
                return Enumerable.Empty<Question>();
            }

            return Repository.Query(q => q.AuthorId == authorId);
        }

        public IEnumerable<Question> CreateBatch(ICollection<TriviaQuestion> questions)
        {
            var created = questions
                .Select(question => Mapper.Map<Question>(question))
                .Select(question => Repository.Create(question)).ToList();

            // Repository.Create(new Question
            // {
            //     AuthorId = "93e584ea-1846-43bc-9906-36beb0cff48c",
            //     Type = QuestionType.Boolean,
            //     Category = QuestionCategory.Animals,
            //     Difficulty = QuestionDifficulty.Easy,
            //     Text = "Anime?",
            //     CorrectAnswers = "Yes",
            //     IncorrectAnswers = "No"
            // });

            Repository.SaveChanges();

            // return created;
            return Enumerable.Empty<Question>();
        }

        private static Question PatchValues(Question original, QuestionDto request)
        {
            original.Text = request.Text;
            original.CorrectAnswers = string.Join(' ', request.CorrectAnswers); // TODO: Fix
            original.Explanation = request.Explanation;

            return original;
        }
    }
}