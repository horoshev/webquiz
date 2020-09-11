using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<Question> GetQuestionsByQuery(QuestionQuery? query)
        {
            query ??= new QuestionQuery();

            return Repository.Query(query.Condition);
        }

        public Task<Page<Question>> GetQuestionPageByQuery(QuestionQuery? query)
        {
            query ??= new QuestionQuery();

            return Repository.Query(query);
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

            Repository.SaveChanges();

            return Enumerable.Empty<Question>();
        }

        private static Question PatchValues(Question original, QuestionDto request)
        {
            original.Text = request.Text;
            original.CorrectAnswers = Question.JoinAnswers(request.CorrectAnswers);
            original.Explanation = request.Explanation;

            return original;
        }
    }
}