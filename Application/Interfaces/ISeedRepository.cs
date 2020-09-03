namespace Application.Interfaces
{
    public interface ISeedRepository
    {
        /// <summary>
        /// Generation of fake questions.
        /// </summary>
        /// <param name="count">Number of generated questions.</param>
        void GenerateQuestions(int count = 10);
    }
}