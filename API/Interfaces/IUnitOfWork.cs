namespace API.Interfaces;

public interface IUnitOfWork
{
    ITokenRepository TokenRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}
