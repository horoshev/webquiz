using System.Collections.Generic;
using Application.Entities;

namespace Application.Interfaces
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> GetQuestions();
        Question GetQuestionById(int questionId);
        void InsertQuestion(Question question);
        void DeleteQuestion(int questionId);
        void UpdateQuestion(Question question);
        void Save();
    }
}