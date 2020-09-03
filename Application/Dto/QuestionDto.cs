using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Entities;
using Application.Interfaces;

namespace Application.Dto
{
    public class QuestionDto : IRepositoryEntity
    {
        public int Id { get; set; }
        public string? AuthorId { get; set; }
        public QuestionType Type { get; set; }
        public QuestionCategory Category { get; set; }
        public QuestionDifficulty Difficulty { get; set; }

        [MinLength(1)]
        public string Text { get; set; } = "";
        public string Explanation { get; set; } = "";

        [Required]
        public ICollection<string> CorrectAnswers { get; set; }

        [Required]
        public ICollection<string> IncorrectAnswers { get; set; } = new List<string>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{Text} ({string.Join('/', CorrectAnswers)}) created by {AuthorId}";
        }
    }
}