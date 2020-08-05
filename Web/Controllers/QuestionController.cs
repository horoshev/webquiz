using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Application.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _repository;
        private readonly ISeedRepository _seedRepository;
        private readonly UserManager<User> _userManager;

        public QuestionController(
            IQuestionRepository repository,
            ISeedRepository seedRepository,
            UserManager<User> userManager)
        {
            _repository = repository;
            _seedRepository = seedRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<Question> GetQuestions()
        {
            return _repository.GetQuestions();
        }

        [HttpGet]
        [Route("{questionId}")]
        public IActionResult GetQuestionById(int questionId)
        {
            var question = _repository.GetQuestionById(questionId);

            if (question is null) return NotFound();

            return Ok(question);
        }

        [Authorize]
        [HttpGet]
        [Route("random")]
        public async Task<IActionResult> GetRandomQuestion()
        {
            var user = await _userManager.GetUserAsync(User);

            var question = _repository.GetQuestionById(1);
            if (question is null) return NotFound();

            return Ok(question);
        }

        [Authorize]
        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            var username = _userManager.GetUserName(User);

            // Console.WriteLine(user is null);
            // Console.WriteLine(User.IsAuthenticated());
            // Console.WriteLine(username);
            // Console.WriteLine(HttpContext.User.Identity?.Name);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult InsertQuestion(Question question)
        {
            var author = new Author(); // 

            if (question is null || author is null) return BadRequest();

            _repository.InsertQuestion(question);

            return Ok();
        }

        [HttpPost]
        [Route("{count}")]
        public IActionResult GenerateQuestions(int count)
        {
            _seedRepository.GenerateQuestions(count);

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteQuestion(int questionId)
        {
            if (_repository.GetQuestionById(questionId) is null) return BadRequest();

            _repository.DeleteQuestion(questionId);

            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateQuestion(Question question)
        {
            if (question is null) return BadRequest();
            if (_repository.GetQuestionById(question.Id) is null) return NotFound();

            _repository.UpdateQuestion(question);

            return NoContent();
        }

        public void Save()
        {

        }
    }
}