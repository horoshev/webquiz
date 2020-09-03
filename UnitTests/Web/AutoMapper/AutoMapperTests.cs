using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Entities;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Web.AutoMapper
{
    public class AutoMapperTests
    {
        private static readonly IMapper Mapper = Utils.Mapper;
        private static readonly User TestUser = new User
        {
            Id = "TestId"
        };

        private static readonly Question TestQuestion = new Question
        {
            Id = 3,
            AuthorId = TestUser.Id,
            Text = "test text",
            CorrectAnswers = @"a|b|c|\|pipe|damn", // should parsed to a, b, c, |pipe, damn.
            IncorrectAnswers = @"e|f|mid\||neck", // should parsed to e, f, mid|, neck.
            Explanation = "test explanation",
            CreatedAt = DateTime.Today.AddDays(-2),
            UpdatedAt = DateTime.Today
        };

        private static readonly QuestionDto DtoOfTestQuestion = new QuestionDto
        {
            Id = TestQuestion.Id,
            AuthorId = TestQuestion.AuthorId,
            Text = TestQuestion.Text,
            CorrectAnswers = Question.SplitAnswers(TestQuestion.CorrectAnswers).ToList(),
            IncorrectAnswers = Question.SplitAnswers(TestQuestion.IncorrectAnswers).ToList(),
            Explanation = TestQuestion.Explanation,
            CreatedAt = TestQuestion.CreatedAt,
            UpdatedAt = TestQuestion.UpdatedAt
        };

        private static readonly QuestionDto TestDto = new QuestionDto
        {
            Id = 8,
            AuthorId = "4",
            Text = "dto test",
            CorrectAnswers = new List<string> {"1", "2", "3"},
            IncorrectAnswers = new List<string> {"4", "5", "6"},
            Explanation = "dto explanation",
            CreatedAt = DateTime.Today.AddDays(-4),
            UpdatedAt = DateTime.Today.AddDays(-2)
        };

        private static readonly Question QuestionOfTestDto = new Question
        {
            Id = TestDto.Id,
            AuthorId = TestDto.AuthorId,
            Text = TestDto.Text,
            CorrectAnswers = Question.JoinAnswers(TestDto.CorrectAnswers),
            IncorrectAnswers = Question.JoinAnswers(TestDto.IncorrectAnswers),
            Explanation = TestDto.Explanation,
            CreatedAt = TestDto.CreatedAt ?? DateTime.Now,
            UpdatedAt = TestDto.UpdatedAt ?? DateTime.Now
        };

        [Test]
        public void Forward()
        {
            var dto = Mapper.Map<QuestionDto>(TestQuestion);

            dto.Should().BeEquivalentTo(DtoOfTestQuestion);
        }

        [Test]
        public void Reverse()
        {
            var question = Mapper.Map<Question>(TestDto);

            question.Should().Match<Question>(actual =>
                actual.Id == QuestionOfTestDto.Id &&
                actual.CorrectAnswers == QuestionOfTestDto.CorrectAnswers &&
                actual.IncorrectAnswers == QuestionOfTestDto.IncorrectAnswers &&
                actual.Explanation == QuestionOfTestDto.Explanation &&
                actual.Text == QuestionOfTestDto.Text &&
                actual.CreatedAt == QuestionOfTestDto.CreatedAt &&
                actual.UpdatedAt == QuestionOfTestDto.UpdatedAt &&
                actual.AuthorId == QuestionOfTestDto.AuthorId
            );
        }

        private static (string, string[])[] _answersSplitCases = new[]
        {
            (@"a|b|c|\|pipe|damn", new[] {"a", "b", "c", "|pipe", "damn"}),
            (@"e|f|mid\||neck", new[] {"e", "f", "mid|", "neck"}),
            (@"r|v|we\|m|end", new[] {"r", "v", "we|m", "end"}),
        };

        [TestCaseSource(nameof(_answersSplitCases))]
        public void SplitAnswersCorrectly((string, string[]) testCase)
        {
            var (source, expected) = testCase;

            var actual = Question.SplitAnswers(source);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}