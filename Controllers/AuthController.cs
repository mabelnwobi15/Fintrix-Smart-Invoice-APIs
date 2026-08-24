using Microsoft.AspNetCore.Mvc;
using SmartInvoice.API.Data;
using Microsoft.AspNetCore.Identity;
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

    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    
    [HttpPost("register")]
public IActionResult Register(RegisterDto dto)
{
    if (_context.Users.Any(u => u.Email == dto.Email))
        return Conflict("Email already exists");

    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email
    };

    user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

    _context.Users.Add(user);
    _context.SaveChanges();

    return Ok(new
    {
        user.Id,
        user.Name,
        user.Email
    });
}

   [HttpPost("login")]
public IActionResult Login(LoginDto dto)
{
    var user = _context.Users
        .FirstOrDefault(u => u.Email == dto.Email);

    if (user == null)
        return Unauthorized("Invalid email or password");

    var result = _passwordHasher.VerifyHashedPassword(
        user,
        user.PasswordHash,
        dto.Password);

    if (result == PasswordVerificationResult.Failed)
        return Unauthorized("Invalid email or password");

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