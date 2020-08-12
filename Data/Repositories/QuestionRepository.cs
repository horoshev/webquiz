using Application.Entities;
using Application.Interfaces;

namespace Data.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly WebQuizDbContext _context;

        public QuestionRepository(WebQuizDbContext context) : base(context)
        {
            _context = context;
        }
    }
}