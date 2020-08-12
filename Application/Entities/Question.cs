using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Application.Entities
{
    [DebuggerDisplay("Id: {Id} / AuthorId: {Author.Id}")]
    public class Question
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; } = "";

        public string Explanation { get; set; } = "";

        [Required]
        [MinLength(1)]
        public string Answers { get; set; } = "";

        [NotMapped]
        public List<Uri> Sources { get; set; }

        [Required]
        public User Author { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}