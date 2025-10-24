using Microsoft.AspNetCore.Mvc;
using DynAmino.Services;
using DynAmino.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace DynAmino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly Dictionary<string, string> _users = new()
        {
            { "admin", "password123" },
            { "user", "password456" }
        };

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (_users.TryGetValue(user.Username, out var password) && password == user.Password)
            {
                var token = _authService.GenerateJwtToken(user.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        [Authorize]
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Token no proporcionado.");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
                var expiration = jwtToken.ValidTo;
                
                return Ok(new
                {
                    Username = userName,
                    Expiration = expiration,
                    Claims = jwtToken.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Token inválido.", details = ex.Message });
            }
        
        }
    }
}
