using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Entities;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FunctionalTests.Data.Seed;
using Newtonsoft.Json;
using Tests;
using Xunit;

namespace FunctionalTests.Web.Controllers
{
    public class QuestionControllerTests : IClassFixture<WebTestFixture>
    {
        public QuestionControllerTests(WebTestFixture factory)
        {
            Factory = factory; //.Build();
        }

        private WebTestFixture Factory { get; }

        private readonly Func<EquivalencyAssertionOptions<QuestionDto>, EquivalencyAssertionOptions<QuestionDto>> _excludeDate =
            options => options.Excluding(info => info.SelectedMemberInfo.MemberType == typeof(DateTime?));

        [Fact]
        public async Task ReturnsUnauthorizedGivenAnonymousUser()
        {
            var anonymousUser = Factory.CreateClient();

            var response = await anonymousUser.GetAsync("api/question/random");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ReturnsOkForAuthorizedUser()
        {
            var authenticatedUser = WebTestFixture.AuthenticateUser(Factory.CreateClient());

            var response = await authenticatedUser.GetAsync("api/question/random");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // ToDo: Fix issue with database changes during the tests
        [Fact]
        public async Task _ReturnsOkGivenAnonymousUser()
        {
            var anonymousUser = Factory.CreateClient();

            var response = await anonymousUser.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }

        [Fact]
        public async Task GetExistingQuestionById()
        {
            const int questionId = 1;
            var authenticatedUser = WebTestFixture.AuthenticateUser(Factory.CreateClient());

            var response = await authenticatedUser.GetAsync($"api/question/{questionId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<QuestionDto>(body);

            var testQuestion = QuestionContextSeed.TestQuestions.First(q => q.Id == questionId);
            var expected = Utils.Mapper.Map<QuestionDto>(testQuestion);

            actual.Should().BeEquivalentTo(expected, _excludeDate);
        }

        [Fact]
        public async Task GetNonExistingQuestionById()
        {
            const int questionId = 666;
            var authenticatedUser = WebTestFixture.AuthenticateUser(Factory.CreateClient());

            var response = await authenticatedUser.GetAsync($"api/question/{questionId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ControllerShouldReturnCreatedQuestion()
        {
            var newQuestion = new QuestionDto
            {
                Type = QuestionType.Boolean,
                Difficulty = QuestionDifficulty.Hard,
                Category = QuestionCategory.Books,
                Text = "Are you read books?",
                Explanation = "There is no right answer.",
                CorrectAnswers = new List<string> {"No"},
                IncorrectAnswers = new List<string> {"Yes"}
            };
            var request = new StringContent(JsonConvert.SerializeObject(newQuestion), Encoding.UTF8, "application/json");
            var authenticatedUser = WebTestFixture.AuthenticateUser(Factory.CreateClient());

            var response = await authenticatedUser.PostAsync("api/question/", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<QuestionDto>(body);

            content.Should().BeEquivalentTo(newQuestion,
                options => options.Excluding(dto => dto.Id).Excluding(dto => dto.AuthorId)
                    .Excluding(info => info.SelectedMemberInfo.MemberType == typeof(DateTime?)));

            content.Id.Should().BeGreaterThan(QuestionContextSeed.TestQuestions.Count());
            content.AuthorId.Should().Be("123", "this value set in auth handler");

            // Clean Up
            response = await authenticatedUser.DeleteAsync($"api/question/{content.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public static TheoryData<(string userId, IEnumerable<Question> expected)> AuthorQuestionTestCases => new TheoryData<(string userId, IEnumerable<Question> expected)>
        {
            ("1", QuestionContextSeed.TestQuestions.Where(q => q.AuthorId == "1")),
            ("2", QuestionContextSeed.TestQuestions.Where(q => q.AuthorId == "2")),
            ("42", QuestionContextSeed.TestQuestions.Where(q => q.AuthorId == "42")),
        };

        [Theory]
        [MemberData(nameof(AuthorQuestionTestCases))]
        public async Task GetQuestionsWithAuthorId((string userId, IEnumerable<Question> expected) testCase)
        {
            var (userId, expected) = testCase;
            var author = WebTestFixture.AuthenticateUser(Factory.CreateClient(), userId);

            var response = await author.GetAsync("api/question/author");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            var expectedDto = Utils.Mapper.Map<IEnumerable<Question>, IEnumerable<QuestionDto>>(expected);
            actual.Should().BeEquivalentTo(expectedDto, _excludeDate);
        }

        [Fact]
        public async Task PolicyShouldReturnForbidden()
        {
            var question = QuestionContextSeed.TestQuestions.First();
            var author = WebTestFixture.AuthenticateUser(Factory.CreateClient(), question.AuthorId + 1);

            var response = await author.DeleteAsync($"api/question/{question.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteShouldReturnNoContent()
        {
            var question = QuestionContextSeed.TestQuestions.First();
            var author = WebTestFixture.AuthenticateUser(Factory.CreateClient(), question.AuthorId);

            var response = await author.DeleteAsync($"api/question/{question.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteShouldReturnBadRequest()
        {
            var question = QuestionContextSeed.TestQuestions.First();
            var author = WebTestFixture.AuthenticateUser(Factory.CreateClient(), question.AuthorId);

            var response = await author.DeleteAsync($"api/question/{112233}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}