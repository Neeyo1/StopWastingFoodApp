using API.Interfaces;

namespace API.Data;

public class UnitOfWork(DataContext context, ITokenRepository tokenRepository,
    ICategoryRepository categoryRepository, IProductRepository productRepository) : IUnitOfWork
{
    public ITokenRepository TokenRepository => tokenRepository;
    public ICategoryRepository CategoryRepository => categoryRepository;
    public IProductRepository ProductRepository => productRepository;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
