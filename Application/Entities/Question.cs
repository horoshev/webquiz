using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Application.Interfaces;

namespace Application.Entities
{
    [DebuggerDisplay("Id: {Id} / AuthorId: {AuthorId}")]
    public class Question : IRepositoryEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        [Required]
        public QuestionCategory Category { get; set; } = QuestionCategory.None;

        [Required]
        public QuestionDifficulty Difficulty { get; set; } = QuestionDifficulty.None;

        [Required]
        [MinLength(1)]
        public string Text { get; set; } = "";
        public string Explanation { get; set; } = "";

        [Required]
        [MinLength(1)]
        public string CorrectAnswers { get; set; } // Multiple answers stored in database like: 'answer1|answer2|answer3'
                                                   // Pipe should be interpolated with backslash like: \| or \n
        [Required]
        public string IncorrectAnswers { get; set; } = ""; // Same storage technique like 'CorrectAnswers'

        public string Sources { get; set; } = "";

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public static IEnumerable<string> SplitAnswers(string answers)
        {
            return Regex.Split(answers, @"(?<=[\w|])[|](?=[\w\\]+)").Select(item => item.Replace(@"\|", "|"));
        }

        // TODO: Fix case ' '
        public static string JoinAnswers(IEnumerable<string> answers)
        {
            var items = answers.Select(item => item.Replace("|", @"\|"));

            return string.Join('|', items);
        }
    }
}