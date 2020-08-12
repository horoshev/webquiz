using System.Collections.Generic;
using Application.Dto;
using Application.Entities;
using Application.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        private readonly UserManager<User> _userManager;

        public QuestionController(
            IMapper mapper,
            IQuestionService questionService,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _questionService = questionService;
            _userManager = userManager;
        }

        // TODO: Pagination
        [HttpGet]
        public IEnumerable<QuestionDto> GetQuestions()
        {
            return _mapper.Map<IEnumerable<QuestionDto>>(_questionService.GetAll());
        }

        [HttpGet]
        [Route("{questionId}")]
        public IActionResult GetQuestionById(int questionId)
        {
            var question = _questionService.Get(questionId);

            if (question is null) return NotFound();

            return Ok(_mapper.Map<QuestionDto>(question));
        }

        [Authorize]
        [HttpGet]
        [Route("random")]
        public IActionResult GetRandomQuestion()
        {
            var question = _questionService.GetRandomQuestion();

            if (question is null) return NotFound();

            return Ok(_mapper.Map<QuestionDto>(question));
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
        [HttpGet]
        [Route("author")]
        public IEnumerable<QuestionDto> GetAuthorQuestions()
        {
            var id = User.GetSubjectIdentifier();

            var questions = _questionService.GetQuestionByAuthorId(id);

            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

        [Authorize]
        [HttpPost]
        public IActionResult InsertQuestion(QuestionDto question)
        {
            var createdQuestion = _questionService.Create(question);

            if (createdQuestion is null) return BadRequest();

            return Ok(createdQuestion.Id.ToString());
        }

        [Authorize(nameof(QuestionPolicy))]
        [HttpDelete]
        [Route("{questionId}")]
        public IActionResult DeleteQuestion(int questionId)
        {
            var deletedQuestion = _questionService.Delete(questionId);

            if (deletedQuestion is null) return BadRequest();

            return NoContent();
        }

        [Authorize(nameof(QuestionPolicy))]
        [HttpPut]
        public IActionResult UpdateQuestion(QuestionDto question)
        {
            var updatedQuestion = _questionService.Update(question);

            if (updatedQuestion is null) return BadRequest();

            return NoContent();
        }
    }
}