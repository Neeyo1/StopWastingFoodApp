using API.DTOs.Inventory;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class InventoryRepository(DataContext context, IMapper mapper) : IInventoryRepository
{
    public void AddInventory(Inventory inventory)
    {
        context.Inventories.Add(inventory);
    }

    public void DeleteInventory(Inventory inventory)
    {
        context.Inventories.Remove(inventory);
    }

    public async Task<IEnumerable<InventoryDto>> GetInventoriesAsync(int householdId)
    {
        return await context.Inventories
            .Where(x => x.HouseholdId == householdId)
            .ProjectTo<InventoryDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Inventory?> GetInventoryByIdAsync(int inventoryId)
    {
        return await context.Inventories
            .FindAsync(inventoryId);
    }

    public async Task<Inventory?> GetInventoryDetailedByIdAsync(int inventoryId)
    {
        return await context.Inventories
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == inventoryId);
    }
}
