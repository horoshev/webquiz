using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly TriviaQuestionService _trivia = new TriviaQuestionService();
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public SeedController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("{count}")]
        public async Task<IActionResult> GenerateQuestions(int count)
        {
            var questions = await _trivia.GetBooleanQuestion(count);
            var created = _questionService.CreateBatch(questions);

            return Ok(created);
        }
    }
}