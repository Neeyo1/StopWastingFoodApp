using API.DTOs.Household;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class HouseholdsController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HouseholdDto>>> GetHouseholds()
    {
        var households = await unitOfWork.HouseholdRepository.GetHouseholdsAsync();
        
        return Ok(households);
    }

    [HttpGet("{householdId}")]
    public async Task<ActionResult<HouseholdDto>> GetHouseholdById(int householdId)
    {
        var household = await unitOfWork.HouseholdRepository.GetHouseholdDetailedByIdAsync(householdId);
        if (household == null) return NotFound();

        return Ok(mapper.Map<HouseholdDto>(household));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<HouseholdDto>> CreateHousehold(HouseholdCreateDto householdCreateDto)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var household = mapper.Map<Household>(householdCreateDto);
        household.Owner = user;

        unitOfWork.HouseholdRepository.AddHousehold(household);

        var userHousehold = new UserHousehold
        {
            User = user,
            Household = household
        };

        household.UserHouseholds.Add(userHousehold);

        if (await unitOfWork.Complete()) return Ok(mapper.Map<HouseholdDto>(household));
        return BadRequest("Failed to create household");
    }

    [Authorize]
    [HttpPut("{householdId}")]
    public async Task<ActionResult<HouseholdDto>> UpdateHousehold(HouseholdCreateDto householdUpdateDto, int householdId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var household = await unitOfWork.HouseholdRepository.GetHouseholdByIdAsync(householdId);
        if (household == null) return BadRequest("Failed to find household");
        if (household.OwnerId != user.Id) return Unauthorized();

        mapper.Map(householdUpdateDto, household);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to edit household");
    }

    [Authorize]
    [HttpDelete("{householdId}")]
    public async Task<ActionResult> DeleteHousehold(int householdId)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var household = await unitOfWork.HouseholdRepository.GetHouseholdByIdAsync(householdId);
        if (household == null) return BadRequest("Failed to find household");
        if (household.OwnerId != user.Id) return Unauthorized();
        
        unitOfWork.HouseholdRepository.DeleteHousehold(household);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to delete household");
    }

    [Authorize]
    [HttpPost("{householdId}/add-member")]
    public async Task<ActionResult<HouseholdDto>> AddMember(int householdId, [FromQuery] string username)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var userToAdd = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (userToAdd == null) return BadRequest("Could not find user to add");

        var household = await unitOfWork.HouseholdRepository.GetHouseholdByIdAsync(householdId);
        if (household == null) return BadRequest("Could not find household");
        if (household.OwnerId != user.Id) return Unauthorized();

        if (await unitOfWork.HouseholdRepository.GetUserHouseholdByIdsAsync(userToAdd.Id, householdId) != null)
            return BadRequest("This user is already member of this household");

        var userHousehold = new UserHousehold
        {
            User = userToAdd,
            Household = household
        };

        household.UserHouseholds.Add(userHousehold);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to add member to household");
    }

    [Authorize]
    [HttpPost("{householdId}/remove-member")]
    public async Task<ActionResult<HouseholdDto>> RemoveMember(int householdId, [FromQuery] string username)
    {
        var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var userToRemove = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (userToRemove == null) return BadRequest("Could not find user to add");

        var household = await unitOfWork.HouseholdRepository.GetHouseholdByIdAsync(householdId);
        if (household == null) return BadRequest("Could not find household");
        if (household.OwnerId != user.Id) return Unauthorized();
        if (household.OwnerId == userToRemove.Id) return BadRequest("Ypu cannot remove yourself");

        var userHousehold = await unitOfWork.HouseholdRepository.GetUserHouseholdByIdsAsync
            (userToRemove.Id, householdId);
        if (userHousehold == null)
            return BadRequest("This user is not member of this household");

        household.UserHouseholds.Remove(userHousehold);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to remove member from household");
    }
}
