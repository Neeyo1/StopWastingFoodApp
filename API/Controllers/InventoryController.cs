using API.DTOs.Inventory;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class InventoryController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventories(int householdId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");
        if (!await CanUserAccessInventory(user.Id, householdId)) return Unauthorized();

        var inventories = await unitOfWork.InventoryRepository.GetInventoriesAsync(householdId);
        
        return Ok(inventories);
    }

    [HttpGet("{inventoryId}")]
    public async Task<ActionResult<InventoryDto>> GetInventoryById(int inventoryId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var inventory = await unitOfWork.InventoryRepository.GetInventoryDetailedByIdAsync(inventoryId);
        if (inventory == null) return NotFound();
        if (!await CanUserAccessInventory(user.Id, inventory.HouseholdId)) return Unauthorized();

        return Ok(mapper.Map<InventoryDto>(inventory));
    }

    [HttpPost]
    public async Task<ActionResult<InventoryDto>> CreateInventory(InventoryCreateDto inventoryCreateDto)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var household = await unitOfWork.HouseholdRepository.GetHouseholdByIdAsync(inventoryCreateDto.HouseholdId);
        if (household == null) return BadRequest("Could not find household");

        var product = await unitOfWork.ProductRepository.GetProductByIdWithDetailsAsync(inventoryCreateDto.ProductId);
        if (product == null) return BadRequest("Could not find product");

        if (!await CanUserAccessInventory(user.Id, household.Id)) return Unauthorized();

        var inventory = mapper.Map<Inventory>(inventoryCreateDto);
        if (inventory.ExpireDate > DateTime.UtcNow.AddDays(3))
        {
            inventory.Status = FreshStatus.Fresh;
        }
        else if(inventory.ExpireDate > DateTime.UtcNow)
        {
            inventory.Status = FreshStatus.ExpireSoon;
        }
        else
        {
            inventory.Status = FreshStatus.Expired;
        }

        unitOfWork.InventoryRepository.AddInventory(inventory);

        if (await unitOfWork.Complete()) return Ok(mapper.Map<InventoryDto>(inventory));
        return BadRequest("Failed to create inventory");
    }

    [HttpPut("{inventoryId}")]
    public async Task<ActionResult<InventoryDto>> UpdateInventory(
        InventoryUpdateDto inventoryUpdateDto, int inventoryId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var inventory = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(inventoryId);
        if (inventory == null) return BadRequest("Failed to find inventory");
        if (!await CanUserAccessInventory(user.Id, inventory.HouseholdId)) return Unauthorized();

        inventory.ExpireDate = inventoryUpdateDto.ExpireDate;
        if (inventory.ExpireDate > DateTime.UtcNow.AddDays(3))
        {
            inventory.Status = FreshStatus.Fresh;
        }
        else if(inventory.ExpireDate > DateTime.UtcNow)
        {
            inventory.Status = FreshStatus.ExpireSoon;
        }
        else
        {
            inventory.Status = FreshStatus.Expired;
        }

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to edit inventory");
    }

    [HttpPost("{inventoryId}/set-as-consumed")]
    public async Task<ActionResult> SetInventoryAsConsumed(int inventoryId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var inventory = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(inventoryId);
        if (inventory == null) return BadRequest("Failed to find inventory");
        if (!await CanUserAccessInventory(user.Id, inventory.HouseholdId)) return Unauthorized();
        
        inventory.Status = FreshStatus.Consumed;

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to set inventory as consumed");
    }

    [HttpPost("{inventoryId}/set-as-thrown-out")]
    public async Task<ActionResult> SetInventoryAsThrownOut(int inventoryId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var inventory = await unitOfWork.InventoryRepository.GetInventoryByIdAsync(inventoryId);
        if (inventory == null) return BadRequest("Failed to find inventory");
        if (!await CanUserAccessInventory(user.Id, inventory.HouseholdId)) return Unauthorized();
        
        inventory.Status = FreshStatus.ThrownOut;

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to set inventory as consumed");
    }

    private async Task<bool> CanUserAccessInventory(int userId, int householdId)
    {
        var userHousehold = await unitOfWork.HouseholdRepository.GetUserHouseholdByIdsAsync(userId, householdId);
        return userHousehold != null;
    }
}
