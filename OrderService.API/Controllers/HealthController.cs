using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Persistence;

namespace OrderService.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;

    public HealthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // Testa conexão
            var canConnect = await _context.Database.CanConnectAsync();

            if (!canConnect)
                return StatusCode(500, new { status = "erro", message = "Não conectou no banco" });

            // Teste real (query simples)
            var result = await _context.Database.ExecuteSqlRawAsync("SELECT 1");

            return Ok(new
            {
                status = "ok",
                database = "connected",
                result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "erro",
                message = ex.Message
            });
        }
    }
}