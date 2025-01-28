namespace API.Interfaces;

public interface IUnitOfWork
{
    ITokenRepository TokenRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    IHouseholdRepository HouseholdRepository { get; }
    IUserRepository UserRepository { get; }
    IInventoryRepository InventoryRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}
