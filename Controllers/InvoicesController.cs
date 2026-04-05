using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInvoice.API.Data;
using SmartInvoice.API.DTOs;
using SmartInvoice.API.Models;
using System.Security.Claims;
using SmartInvoice.API.Services;

namespace SmartInvoice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoicesController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public IActionResult CreateInvoice(CreateInvoiceDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // user is logged-in they must create the invoice

        // Calculate total
        decimal totalAmount = dto.Items.Sum(i => i.Quantity * i.Price);

        var invoice = new Invoice
        {
            ClientId = dto.ClientId,
            UserId = userId,
            Date = DateTime.Now,
            DueDate = dto.DueDate,
            Status = "Pending",
            TotalAmount = totalAmount
        };

        _context.Invoices.Add(invoice);
        _context.SaveChanges();

        foreach (var item in dto.Items)
        {
            var invoiceItem = new InvoiceItem
            {
                InvoiceId = invoice.Id,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price
            };

            _context.InvoiceItems.Add(invoiceItem);
        }

        _context.SaveChanges();

        return Ok(invoice);
    }

    [HttpGet]
    public IActionResult GetInvoices()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var invoices = _context.Invoices
            .Where(i => i.UserId == userId)
            .Select(i => new
            {
                i.Id,
                i.Date,
                i.DueDate,
                i.Status,
                i.TotalAmount,
                ClientName = _context.Clients
                    .Where(c => c.Id == i.ClientId)
                    .Select(c => c.Name)
                    .FirstOrDefault()
            })
            .ToList();

        return Ok(invoices);
    }


    [HttpGet("{id}/pdf")]
    public IActionResult DownloadInvoicePdf(int id, [FromServices] PdfService pdfService)
    {
    var pdfBytes = pdfService.GenerateInvoicePdf(id);

    return File(pdfBytes, "application/pdf", $"Invoice_{id}.pdf");
    }
    }