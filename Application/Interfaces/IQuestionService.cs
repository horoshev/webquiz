using System.Collections.Generic;
using Application.Dto;
using Application.Entities;
using Application.Services;

namespace Application.Interfaces
{
    // TODO: Paging and Sorting
    public interface IQuestionService : IBaseOperations<QuestionDto, Question>
    {
        /// <summary>
        /// Select single question from all and returning it.
        /// </summary>
        /// <returns>Random question or null if there is no questions.</returns>
        Question? GetRandomQuestion();

        /// <summary>
        /// Returning all questions matches question query parameters.
        /// </summary>
        /// <param name="query">Query with question parameters.</param>
        /// <returns>Questions matches query parameters.</returns>
        IEnumerable<Question> GetQuestionsByQuery(QuestionQuery query);

        /// <summary>
        /// Returning all questions with provided authorId.
        /// </summary>
        /// <param name="authorId">Identifier of question author.</param>
        /// <returns>Questions matches authorId.</returns>
        IEnumerable<Question> GetQuestionByAuthorId(string authorId);

        /// <summary>
        /// Adding multiple questions from 3rd party API.
        /// </summary>
        /// <param name="questions">Questions to add in data source.</param>
        /// <returns>Questions added to data source.</returns>
        IEnumerable<Question> CreateBatch(ICollection<TriviaQuestion> questions);
    }
}