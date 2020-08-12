using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Validators;

namespace Application.Dto
{
    public class QuestionDto
    {
        [MinLength(1)]
        public string Text { get; set; } = "";
        public string Explanation { get; set; } = "";

        [Collection(MinElementLength = 1, MinLength = 1)]
        public ICollection<string> Answers { get; set; } = new List<string>();

        public int Id { get; set; }
        public string AuthorId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}