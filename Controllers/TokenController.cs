using Microsoft.AspNetCore.Mvc;
using DynAmino.Services;

namespace DynAmino.Controllers;

[ApiController]
[Route("api/")]
public class TokenController : ControllerBase
{
    private readonly IAuthService _authService;

    public TokenController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("get-token")]
    public async Task<IActionResult> GetToken()
    {
        try
        {
            var tokenResponse = await _authService.GetTokenAsync();
            return Ok(tokenResponse);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(new { message = "Server Error", error = ex.Message });
        }
    }

}