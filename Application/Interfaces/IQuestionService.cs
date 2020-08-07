using Application.Entities;

namespace Application.Interfaces
{
    public interface IQuestionService : IQuestionRepository
    {
        Question GetRandomQuestion();
    }
}