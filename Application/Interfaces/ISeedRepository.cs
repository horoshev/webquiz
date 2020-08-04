namespace Application.Interfaces
{
    public interface ISeedRepository
    {
        void GenerateQuestions(int count = 10);
    }
}