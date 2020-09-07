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
            AnonymousUser = factory.CreateClient();
            AuthenticatedUser = WebTestFixture.AuthenticateUser(factory.CreateClient());
        }

        private HttpClient AnonymousUser { get; }
        private HttpClient AuthenticatedUser { get; }

        [Fact(Skip = "Fail")]
        public async Task ReturnsRedirectGivenAnonymousUser()
        {
            var response = await AnonymousUser.GetAsync("weatherforecast");
            var redirectLocation = response.Headers.Location.OriginalString;

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            redirectLocation.Should().Contain("/Account/Login");
        }

        [Fact]
        public async Task ReturnsUnauthorizedGivenAnonymousUser()
        {
            var response = await AnonymousUser.GetAsync("api/question/random");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ReturnsOkForAuthorizedUser()
        {
            var response = await AuthenticatedUser.GetAsync("api/question/random");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(Skip = "Number of questions is changed during the test.")]
        public async Task ReturnsOkGivenAnonymousUser()
        {
            var response = await AnonymousUser.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }

        [Fact]
        public async Task GetExistingQuestionById()
        {
            const int questionId = 1;

            var response = await AuthenticatedUser.GetAsync($"api/question/{questionId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<QuestionDto>(body);

            var testQuestion = QuestionContextSeed.TestQuestions.First(q => q.Id == questionId);
            var expected = Utils.Mapper.Map<QuestionDto>(testQuestion);

            actual.Should().BeEquivalentTo(expected, options => options.Excluding(info => info.SelectedMemberInfo.MemberType == typeof(DateTime?)));
        }

        [Fact]
        public async Task GetNonExistingQuestionById()
        {
            const int questionId = 666;

            var response = await AuthenticatedUser.GetAsync($"api/question/{questionId}");

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
            var response = await AuthenticatedUser.PostAsync("api/question/", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<QuestionDto>(body);

            content.Should().BeEquivalentTo(newQuestion,
                options => options.Excluding(dto => dto.Id).Excluding(dto => dto.AuthorId)
                    .Excluding(info => info.SelectedMemberInfo.MemberType == typeof(DateTime?)));
        }

        [Fact(Skip = "Fail")]
        public async Task DeleteShouldReturnNoContent()
        {
            var response = await AuthenticatedUser.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }

        [Fact(Skip = "Fail")]
        public async Task DeleteShouldReturnBadRequest()
        {
            var response = await AuthenticatedUser.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }
    }
}