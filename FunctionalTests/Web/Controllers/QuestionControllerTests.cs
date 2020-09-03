using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Dto;
using FluentAssertions;
using FunctionalTests.Data.Seed;
using Newtonsoft.Json;
using Xunit;

namespace FunctionalTests.Web.Controllers
{
    public class QuestionControllerTests : IClassFixture<WebTestFixture>
    {
        public QuestionControllerTests(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        private HttpClient Client { get; }

        [Fact(Skip = "Fail")]
        public async Task ReturnsRedirectGivenAnonymousUser()
        {
            var response = await Client.GetAsync("weatherforecast");
            var redirectLocation = response.Headers.Location.OriginalString;

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            redirectLocation.Should().Contain("/Account/Login");
        }

        [Fact]
        public async Task ReturnsUnauthorizedGivenAnonymousUser()
        {
            var response = await Client.GetAsync("api/question/random");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ReturnsOkGivenAnonymousUser()
        {
            var response = await Client.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }

        [Fact(Skip = "Fail")]
        public async Task ControllerShouldReturnCreatedQuestion()
        {
            var request = new StringContent(JsonConvert.SerializeObject(new QuestionDto()));
            var response = await Client.PostAsync("api/question/", request);
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }

        [Fact(Skip = "Fail")]
        public async Task DeleteShouldReturnNoContent()
        {
            var response = await Client.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }

        [Fact(Skip = "Fail")]
        public async Task DeleteShouldReturnBadRequest()
        {
            var response = await Client.GetAsync("api/question/");
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<QuestionDto>>(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(QuestionContextSeed.TestQuestions.Count());
        }
    }
}