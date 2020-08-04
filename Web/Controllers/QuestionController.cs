using System.Collections.Generic;
using Application.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _repository;
        private readonly ISeedRepository _seedRepository;

        public QuestionController(IQuestionRepository repository, ISeedRepository seedRepository)
        {
            _repository = repository;
            _seedRepository = seedRepository;
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

        [HttpPost]
        [Authorize]
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