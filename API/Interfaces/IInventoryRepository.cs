using API.DTOs.Inventory;
using API.Entities;

namespace API.Interfaces;

public interface IInventoryRepository
{
    void AddInventory(Inventory inventory);
    void DeleteInventory(Inventory inventory);
    Task<IEnumerable<InventoryDto>> GetInventoriesAsync(int householdId);
    Task<Inventory?> GetInventoryByIdAsync(int inventoryId);
    Task<Inventory?> GetInventoryDetailedByIdAsync(int inventoryId);
}
