using Microsoft.AspNetCore.Mvc;
using SmartInvoice.API.Data;
using SmartInvoice.API.DTOs;
using SmartInvoice.API.Models;
using SmartInvoice.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SmartInvoice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    
    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.Password
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Email == dto.Email && u.PasswordHash == dto.Password);

        if (user == null)
            return Unauthorized();

        var token = _jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    [HttpGet("me")]
[Authorize] // Only accessible with a valid JWT
public IActionResult GetCurrentUser()
{
    // Get userId from JWT claims
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Unauthorized();

    var userId = int.Parse(userIdClaim.Value);

    var user = _context.Users
        .Where(u => u.Id == userId)
        .Select(u => new { u.Name, u.Email })
        .FirstOrDefault();

    if (user == null)
        return NotFound();

    return Ok(user);
}
}