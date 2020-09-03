namespace Application.Interfaces
{
    /// <summary>
    /// Marks that entity can be found in database by id
    /// </summary>
    public interface IRepositoryEntity
    {
        int Id { get; set; }
    }
}