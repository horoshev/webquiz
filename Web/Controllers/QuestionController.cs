using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Common;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ISeedRepository _seedRepository;
        private readonly UserManager<User> _userManager;

        public QuestionController(
            IQuestionService questionService,
            ISeedRepository seedRepository,
            UserManager<User> userManager)
        {
            _questionService = questionService;
            _seedRepository = seedRepository;
            _userManager = userManager;
        }

        // TODO: Pagination
        [HttpGet]
        public IEnumerable<Question> GetQuestions()
        {
            return _questionService.GetQuestions();
        }

        [HttpGet]
        [Route("{questionId}")]
        public IActionResult GetQuestionById(int questionId)
        {
            var question = _questionService.GetQuestionById(questionId);

            if (question is null) return NotFound();

            return Ok(question);
        }

        [Authorize]
        [HttpGet]
        [Route("random")]
        public IActionResult GetRandomQuestion()
        {
            var question = _questionService.GetRandomQuestion();
            if (question is null) return NotFound();

            return Ok(question);
        }

        [Authorize]
        [HttpGet]
        [Route("user")]
        public IActionResult GetCurrentUser()
        {
            // var id = User.GetSubjectIdentifier();
            // var user = await _userManager.FindByIdAsync(id);

            // var jwtid = User.GetSubjectId();
            // var user = await _userManager.GetUserAsync(User); // TODO: Figure out

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertQuestion(QuestionRequest questionRequest)
        {
            var id = User.GetSubjectIdentifier();
            var author = await _userManager.FindByIdAsync(id);

            if (questionRequest is null || author is null) return BadRequest();

            var question = new Question
            {
                Text = questionRequest.Text,
                Answers = questionRequest.Answers.Aggregate((s, i) => $"{s} {i}").Trim(),
                Explanation = questionRequest.Explanation,
                Author = author
            };

            _questionService.InsertQuestion(question);
            _questionService.Save();

            string.IsNullOrEmpty("ds");
            "ds".Contains('X');

            return Ok();
        }

        [HttpPost]
        [Route("{count}")]
        public IActionResult GenerateQuestions(int count)
        {
            _seedRepository.GenerateQuestions(count);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var author = await _userManager.FindByIdAsync(User.GetSubjectIdentifier() ?? "");
            var question = _questionService.GetQuestionById(questionId);

            if (question is null || author is null) return BadRequest();
            if (question.Author.Id != author.Id) return Forbid(); // you can`t delete other people questions

            _questionService.DeleteQuestion(questionId);

            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(Question question)
        {
            var author = await _userManager.FindByIdAsync(User.GetSubjectIdentifier() ?? "");
            if (question is null || author is null) return BadRequest();

            var originalQuestion = _questionService.GetQuestionById(question.Id);
            if (originalQuestion is null) return NotFound();

            if (question.Author.Id != author.Id) return Forbid(); // you can`t modify other people questions

            _questionService.UpdateQuestion(question);

            return NoContent();
        }
    }
}