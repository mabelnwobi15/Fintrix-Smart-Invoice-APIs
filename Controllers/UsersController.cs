using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInvoice.API.Data;
using System.Security.Claims;

namespace SmartInvoice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst("id")?.Value 
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token");

        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest("Invalid user ID format");

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("User not found");

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Email
        });
    }
}