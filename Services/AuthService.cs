using System.Text.Json;
using DynAmino.Dtos.Token;
using DynAmino.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DynAmino.Services;

public class AuthService : IAuthService
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AuthService(ITokenRepository tokenRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _tokenRepository = tokenRepository;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<TokenResponse> GetTokenAsync()
    {
        var credentials = await _tokenRepository.GetAuthTokenCredentialsAsync("PRODUCCION");

        var httpClient = _httpClientFactory.CreateClient();
        using var formData = new MultipartFormDataContent
            {
                { new StringContent(credentials.GrantType), "grant_type" },
                { new StringContent(credentials.Id), "client_id" },
                { new StringContent(credentials.ClientSecret), "client_secret" },
                { new StringContent(credentials.Scope), "scope" }
            };

        using var httpResponseMessage =
        await httpClient.PostAsync(_configuration.GetConnectionString("AminoAuthUrl"), formData);

        httpResponseMessage.EnsureSuccessStatusCode();

        using var contentStream =
        await httpResponseMessage.Content.ReadAsStreamAsync();

        var response = await JsonSerializer.DeserializeAsync<TokenResponse>(contentStream);
        return response ?? throw new Exception("Failed to deserialize token response.");
    }
    
    public string GenerateJwtToken(string username)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(int.Parse(jwtSettings["ExpireDays"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}