using System;
using System.Collections.Generic;
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
            Answers = "a b c d",
            Author = TestUser,
            Explanation = "test explanation",
            Text = "test text",
            CreatedAt = DateTime.Today.AddDays(-2),
            UpdatedAt = DateTime.Today
        };

        private static readonly QuestionDto DtoOfTestQuestion = new QuestionDto
        {
            Id = TestQuestion.Id,
            Answers = TestQuestion.Answers.Split(' '),
            AuthorId = TestQuestion.Author.Id,
            Explanation = TestQuestion.Explanation,
            Text = TestQuestion.Text,
            CreatedAt = TestQuestion.CreatedAt,
            UpdatedAt = TestQuestion.UpdatedAt
        };

        private static readonly QuestionDto TestDto = new QuestionDto
        {
            Id = 8,
            Answers = new List<string> {"1", "2", "3"},
            AuthorId = "4",
            Explanation = "dto explanation",
            Text = "dto test",
            CreatedAt = DateTime.Today.AddDays(-4),
            UpdatedAt = DateTime.Today.AddDays(-2)
        };

        private static readonly Question QuestionOfTestDto = new Question
        {
            Id = TestDto.Id,
            Answers = string.Join(' ', TestDto.Answers),
            Author = new User {Id = TestDto.AuthorId},
            Explanation = TestDto.Explanation,
            Text = TestDto.Text,
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
                actual.Answers == QuestionOfTestDto.Answers &&
                actual.Explanation == QuestionOfTestDto.Explanation &&
                actual.Text == QuestionOfTestDto.Text &&
                actual.CreatedAt == QuestionOfTestDto.CreatedAt &&
                actual.UpdatedAt == QuestionOfTestDto.UpdatedAt &&
                actual.Author.Id == QuestionOfTestDto.Author.Id
            );
        }
    }
}