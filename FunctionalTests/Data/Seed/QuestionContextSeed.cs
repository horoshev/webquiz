using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Data;
using Microsoft.AspNetCore.Identity;

namespace FunctionalTests.Data.Seed
{
    public class QuestionContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, WebQuizDbContext context)
        {
            await SeedUserAsync(userManager);
            await context.Questions.AddRangeAsync(TestQuestions);
            await context.SaveChangesAsync();
        }

        public static async Task SeedUserAsync(UserManager<User> userManager)
        {
            var user = new User
            {
                Email = "test@email.com",
                UserName = "TestUsername",
                LockoutEnabled = false
            };

            var identityResult = await userManager.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                var error = identityResult.Errors.First();
                throw new Exception($"{error.Code}::{error.Description}");
            }

            await userManager.AddPasswordAsync(user, "TestPassword");
        }

        public static IEnumerable<Question> TestQuestions => new []
        {
            new Question
            {
                Id = 1,
                AuthorId = "1",
                Category = QuestionCategory.Anime,
                Difficulty = QuestionDifficulty.Hard,
                Text = "is this test db?",
                CorrectAnswers = "Yes",
                IncorrectAnswers = "No"
            },
            new Question
            {
                Id = 2,
                AuthorId = "2",
                Category = QuestionCategory.Animals,
                Difficulty = QuestionDifficulty.Easy,
                Text = "rly?",
                CorrectAnswers = "Yes",
                IncorrectAnswers = "No"
            },
            new Question
            {
                Id = 3,
                AuthorId = "2",
                Category = QuestionCategory.Art,
                Difficulty = QuestionDifficulty.Medium,
                Text = "no, rly?",
                CorrectAnswers = "Yes",
                IncorrectAnswers = "No"
            },
        };
    }
}