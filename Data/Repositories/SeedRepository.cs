using System;
using System.Linq;
using Application.Entities;
using Application.Interfaces;
using Bogus;

namespace Data.Repositories
{
    public class SeedRepository : ISeedRepository
    {
        private readonly WebQuizDbContext _context;

        public SeedRepository(WebQuizDbContext context)
        {
            _context = context;
        }

        public void GenerateQuestions(int count = 10)
        {
            var faker = new Faker<Question>()
                .RuleFor(x => x.Text, x => x.Lorem.Sentences(3, string.Empty))
                .RuleFor(x => x.Explanation, x => x.Lorem.Sentences(3, string.Empty))
                .RuleFor(x => x.Answers, x => x.Lorem.Words(3).Aggregate((s, w) => $"{s} {w}").Trim())
                .RuleFor(x => x.CreatedAt, x=> DateTime.Now)
                .RuleFor(x => x.UpdatedAt, x=> DateTime.Now)
                .RuleFor(x => x.Author, x => new User
                {
                    Email = x.Person.Email,
                    EmailConfirmed = true,
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                });

            var questions = faker.Generate(count);

            _context.Questions.AddRange(questions);
            _context.SaveChanges();
        }
    }
}