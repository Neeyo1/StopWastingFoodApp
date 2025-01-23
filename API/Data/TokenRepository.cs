using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class TokenRepository(DataContext context) : ITokenRepository
{
    public void AddRefreshToken(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
    }

    public void RemoveRefreshToken(RefreshToken refreshToken)
    {
        context.RefreshTokens.Remove(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshToken(string username, string token)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Username == username && x.Token == token);
    }
}
