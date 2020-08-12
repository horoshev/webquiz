using System.Collections.Generic;
using Application.Dto;
using Application.Entities;

namespace Application.Interfaces
{
    // TODO: Paging and Sorting
    public interface IQuestionService
    {
        IEnumerable<Question> GetAll();
        /// <summary>
        /// Returns question with provided id
        /// </summary>
        /// <param name="entityId">Identifier of the question</param>
        /// <returns>Founded question of null if question was not found</returns>
        Question Get(int entityId);
        Question? Create(QuestionDto entity);
        Question? Update(QuestionDto entity);
        Question? Delete(QuestionDto entity);
        Question? Delete(int entityId);

        Question? GetRandomQuestion();
        IEnumerable<Question> GetQuestionByAuthorId(string authorId);
    }
}