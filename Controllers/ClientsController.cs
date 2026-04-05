using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInvoice.API.Data;
using SmartInvoice.API.DTOs;
using SmartInvoice.API.Models;
using System.Security.Claims;

namespace SmartInvoice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetClients()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var clients = _context.Clients
            .Where(c => c.UserId == userId)
            .Select(c => new ClientResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            })
            .ToList();

        return Ok(clients);
    }

    [HttpPost]
    public IActionResult CreateClient(CreateClientDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var client = new Client
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            UserId = userId 
        };

        _context.Clients.Add(client);
        _context.SaveChanges();

        return Ok(client);
    }
}