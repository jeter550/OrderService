using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OrderService.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult Token()
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        if (string.IsNullOrWhiteSpace(key) ||
            string.IsNullOrWhiteSpace(issuer) ||
            string.IsNullOrWhiteSpace(audience))
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "JWT configuration is missing.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "user")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds,
            claims: claims);

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
