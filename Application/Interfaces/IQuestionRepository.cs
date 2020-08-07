using System.Collections.Generic;
using Application.Entities;
using Application.Services;

namespace Application.Interfaces
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> GetQuestions();
        Question GetQuestionById(int questionId);
        Question GetQuestionByIndex(int index);
        int QuestionsCount();
        void InsertQuestion(Question question);
        void DeleteQuestion(int questionId);
        void UpdateQuestion(Question question);
        void Save();
    }
}