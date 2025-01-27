using API.DTOs.Household;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class HouseholdRepository(DataContext context, IMapper mapper) : IHouseholdRepository
{
    public void AddHousehold(Household household)
    {
        context.Households.Add(household);
    }

    public void DeleteHousehold(Household household)
    {
        context.Households.Remove(household);
    }

    public async Task<IEnumerable<HouseholdDto>> GetHouseholdsAsync()
    {
        return await context.Households
            .Include(x => x.UserHouseholds)
            .ThenInclude(x => x.User)
            .ProjectTo<HouseholdDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Household?> GetHouseholdDetailedByIdAsync(int householdId)
    {
        return await context.Households
            .Include(x => x.UserHouseholds)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == householdId);
    }

    public async Task<Household?> GetHouseholdByIdAsync(int householdId)
    {
        return await context.Households
            .FindAsync(householdId);
    }
}
