using DynAmino.Dtos.Token;

namespace DynAmino.Services;

public interface IAuthService
{
    Task<TokenResponse> GetTokenAsync();
    string GenerateJwtToken(string username);
}
