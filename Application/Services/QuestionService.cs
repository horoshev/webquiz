using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Entities;
using Application.Interfaces;
using AutoMapper;

namespace Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public IEnumerable<Question> GetAll()
        {
            return _questionRepository.GetAll();
        }

        public Question Get(int entityId)
        {
            return _questionRepository.Get(entityId);
        }

        public Question? Create(QuestionDto entity)
        {
            if (entity is null) return null;

            var question = _mapper.Map<Question>(entity);
            // new Question
            // {
            //     Text = questionRequest.Text,
            //     Answers = string.Join(' ', questionRequest.Answers),
            //     Explanation = questionRequest.Explanation,
            //     Author = author
            // };

            question = _questionRepository.Create(question);
            _questionRepository.SaveChanges();

            return question;
        }

        public Question? Update(QuestionDto entity)
        {
            if (entity is null) return null;

            var question = _questionRepository.Get(entity.Id);
            if (question is null) return null;

            question = PatchValues(question, entity); // var patch = _mapper.Map<Question>(entity);
            _questionRepository.Update(question);
            _questionRepository.SaveChanges();

            return question;
        }

        public Question? Delete(QuestionDto entity)
        {
            if (entity is null)
            {
                return null;
            }

            return Delete(entity.Id);
        }

        public Question? Delete(int entityId)
        {
            var question = _questionRepository.Get(entityId);
            if (question is null) return null;

            _questionRepository.Delete(entityId);
            _questionRepository.SaveChanges();

            return question;
        }

        public Question? GetRandomQuestion()
        {
            var count = _questionRepository.Count();
            var index = new Random().Next(count);

            return _questionRepository.GetByIndex(index);
        }

        public IEnumerable<Question> GetQuestionByAuthorId(string authorId)
        {
            if (authorId is null)
            {
                return Enumerable.Empty<Question>();
            }

            return _questionRepository.Query(q => q.Author.Id == authorId);
        }

        private static Question PatchValues(Question original, QuestionDto request)
        {
            original.Text = request.Text;
            original.Answers = string.Join(' ', request.Answers);
            original.Explanation = request.Explanation;

            return original;
        }
    }
}