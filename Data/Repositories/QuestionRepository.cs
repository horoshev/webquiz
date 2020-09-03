using Application.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class QuestionRepository : BaseRepository<Question> , IQuestionRepository
    {
        public QuestionRepository(DbContext context) : base(context)
        {
        }
    }
}