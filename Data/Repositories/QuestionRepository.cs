using System.Collections.Generic;
using System.Linq;
using Application.Entities;
using Application.Interfaces;
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