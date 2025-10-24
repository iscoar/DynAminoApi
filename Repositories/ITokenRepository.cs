using DynAmino.Models;

namespace DynAmino.Repositories;

public interface ITokenRepository
{
    Task<Token> GetAuthTokenCredentialsAsync(string type);
}
