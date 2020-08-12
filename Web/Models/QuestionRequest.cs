namespace Web.Models
{
    public class QuestionRequest
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public string[] Answers { get; set; }
        public string Explanation { get; set; }
    }
}