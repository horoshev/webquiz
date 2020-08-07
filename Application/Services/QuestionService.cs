using System;
using System.Collections.Generic;
using Application.Entities;
using Application.Interfaces;

namespace Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public Question GetRandomQuestion()
        {
            var count = _questionRepository.QuestionsCount();
            var index = new Random().Next(count);

            return _questionRepository.GetQuestionByIndex(index);
        }

        public IEnumerable<Question> GetQuestions()
        {
            return _questionRepository.GetQuestions();
        }

        public Question GetQuestionById(int questionId)
        {
            return _questionRepository.GetQuestionById(questionId);
        }

        public Question GetQuestionByIndex(int index)
        {
            return _questionRepository.GetQuestionByIndex(index);
        }

        public int QuestionsCount()
        {
            return _questionRepository.QuestionsCount();
        }

        public void InsertQuestion(Question question)
        {
            _questionRepository.InsertQuestion(question);
        }

        public void DeleteQuestion(int questionId)
        {
            _questionRepository.DeleteQuestion(questionId);
        }

        public void UpdateQuestion(Question question)
        {
            _questionRepository.UpdateQuestion(question);
        }

        public void Save()
        {
            _questionRepository.Save();
        }
    }
}