using System.Collections.Generic;
using System.Linq;
using Application.Entities;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly WebQuizDbContext _context;

        public QuestionRepository(WebQuizDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Question> GetQuestions()
        {
            return _context.Questions.ToList();
        }

        public Question GetQuestionById(int questionId)
        {
            return _context.Questions.Find(questionId);
        }

        public Question GetQuestionByIndex(int index)
        {
            return _context.Questions.Skip(index).First();
        }

        public int QuestionsCount()
        {
            return _context.Questions.Count();
        }

        public void InsertQuestion(Question question)
        {
            _context.Questions.Add(question);
        }

        public void DeleteQuestion(int questionId)
        {
            var question = _context.Questions.Find(questionId);
            _context.Questions.Remove(question);
        }

        public void UpdateQuestion(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}