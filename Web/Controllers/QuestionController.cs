using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Entities;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Common;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(
            IMapper mapper,
            IQuestionService questionService,
            ILogger<QuestionController> logger
            )
        {
            _mapper = mapper;
            _questionService = questionService;
            _logger = logger;
        }

        [HttpGet("types")]
        public IEnumerable<object> GetQuestionTypes()
        {
            return GetEnumValues<QuestionType>();
        }

        [HttpGet("difficulties")]
        public IEnumerable<object> GetQuestionDifficulties()
        {
            return GetEnumValues<QuestionDifficulty>();
        }

        [HttpGet("categories")]
        public IEnumerable<object> GetQuestionCategories()
        {
            return GetEnumValues<QuestionCategory>();
        }

        private static IEnumerable<object> GetEnumValues<T>() where T : Enum
        {
            foreach (var type in Enum.GetValues(typeof(T)))
            {
                yield return new {value = (int)(type ?? -1), label = type?.ToString() ?? ""};
            }
        }

        [HttpGet]
        public IEnumerable<QuestionDto> GetQuestions([FromQuery] QuestionQuery query, [FromQuery] PagingQuery paging)
        {
            return _mapper.Map<IEnumerable<QuestionDto>>(_questionService.GetQuestionsByQuery(query))
                .Skip(paging.StartPage * paging.PageSize)
                .Take(paging.PageSize);
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
            question.AuthorId = User.GetSubjectIdentifier();

            _logger.LogInformation($"User {User.GetSubjectIdentifier()} is creating the question.");

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
        public IActionResult UpdateQuestion([FromHeader] string userId, QuestionDto question)
        {
            question.AuthorId = userId;

            var updatedQuestion = _questionService.Update(question);

            if (updatedQuestion is null) return BadRequest();

            return NoContent();
        }
    }
}