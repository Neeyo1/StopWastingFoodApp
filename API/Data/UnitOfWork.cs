using API.Interfaces;

namespace API.Data;

public class UnitOfWork(DataContext context, ITokenRepository tokenRepository,
    ICategoryRepository categoryRepository, IProductRepository productRepository,
    IHouseholdRepository householdRepository, IUserRepository userRepository,
    IInventoryRepository inventoryRepository) : IUnitOfWork
{
    public ITokenRepository TokenRepository => tokenRepository;
    public ICategoryRepository CategoryRepository => categoryRepository;
    public IProductRepository ProductRepository => productRepository;
    public IHouseholdRepository HouseholdRepository => householdRepository;
    public IUserRepository UserRepository => userRepository;
    public IInventoryRepository InventoryRepository => inventoryRepository;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
