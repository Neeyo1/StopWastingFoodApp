using API.DTOs.Household;
using API.Entities;

namespace API.Interfaces;

public interface IHouseholdRepository
{
    void AddHousehold(Household household);
    void DeleteHousehold(Household household);
    Task<IEnumerable<HouseholdDto>> GetHouseholdsAsync();
    Task<Household?> GetHouseholdByIdAsync(int householdId);
    Task<Household?> GetHouseholdDetailedByIdAsync(int householdId);
}
