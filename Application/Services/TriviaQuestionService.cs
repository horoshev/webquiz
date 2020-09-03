using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Services
{
    public class TriviaQuestionService
    {
        private const string BaseUrl = "https://opentdb.com/api.php";
        private readonly HttpClient _http = new HttpClient();

        public async Task<ICollection<TriviaQuestion>> GetBooleanQuestion(int amount)
        {
            var parameters = new Dictionary<string, string>
            {
                ["amount"] = amount.ToString(),
                ["type"] = QuestionType.Boolean.ToString().ToLower()
            };
            var url = QueryHelpers.AddQueryString(BaseUrl, parameters);

            Console.WriteLine(url);

            return await _http.GetStringAsync(url)
                .ContinueWith(task => JsonConvert.DeserializeObject<TriviaResponse>(task.Result))
                .ContinueWith(task => task.Result.Results);
        }
    }

    public class TriviaResponse
    {
        public int ResponseCode { get; set; }
        public ICollection<TriviaQuestion> Results { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class TriviaQuestion
    {
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public ICollection<string> IncorrectAnswers { get; set; }
    }
}