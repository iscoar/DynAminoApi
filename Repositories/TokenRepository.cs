using DynAmino.Data;
using DynAmino.Models;
using Microsoft.EntityFrameworkCore;


namespace DynAmino.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _context;

    public TokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Token> GetAuthTokenCredentialsAsync(string type)
    {
        var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Type == type);
        return token ?? throw new KeyNotFoundException($"Token of type {type} not found.");
    }
}
