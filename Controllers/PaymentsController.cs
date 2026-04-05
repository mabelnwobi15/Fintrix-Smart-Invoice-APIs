using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInvoice.API.Data;
using SmartInvoice.API.DTOs;
using SmartInvoice.API.Models;

namespace SmartInvoice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaymentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreatePayment(CreatePaymentDto dto)
    {
        var invoice = _context.Invoices
            .FirstOrDefault(i => i.Id == dto.InvoiceId);

        if (invoice == null)
            return NotFound("Invoice not found");

        if (dto.Amount <= 0)
            return BadRequest("Amount must be greater than 0");

        var payment = new Payment
        {
            InvoiceId = dto.InvoiceId,
            Amount = dto.Amount,
            PaymentDate = DateTime.Now
        };

        _context.Payments.Add(payment);

        _context.SaveChanges();

        var totalPaid = _context.Payments
            .Where(p => p.InvoiceId == dto.InvoiceId)
            .Sum(p => p.Amount);

        if (totalPaid >= invoice.TotalAmount)
        {
            invoice.Status = "Paid";
        }
        else
        {
            if (DateTime.Now > invoice.DueDate)
            {
                invoice.Status = "Overdue";
            }
            else
            {
                invoice.Status = "Pending";
            }
        }

        _context.SaveChanges();

        return Ok(new
        {
            message = "Payment added successfully",
            payment,
            invoiceStatus = invoice.Status,
            totalPaid
        });
    }

    [HttpGet("user")]
public IActionResult GetPaymentsByUser()
{
    var userEmail = User.Identity?.Name; // assuming JWT has email in Name
    if (string.IsNullOrEmpty(userEmail))
        return Unauthorized();

    var payments = _context.Payments
        .Where(p => p.Invoice.Client.Email == userEmail) // assuming Invoice has Client navigation
        .Select(p => new
        {
            p.Id,
            p.Amount,
            p.PaymentDate,
            p.InvoiceId,
            ClientName = p.Invoice.Client.Name,
            ClientEmail = p.Invoice.Client.Email
        })
        .ToList();

    return Ok(payments);
}

    [HttpGet]
    public IActionResult GetAllPayments()
    {
        var payments = _context.Payments
            .Select(p => new
            {
                p.Id,
                p.Amount,
                p.PaymentDate,
                p.InvoiceId
            })
            .ToList();

        return Ok(payments);
    }

    [HttpGet("{invoiceId}")]
    public IActionResult GetPayments(int invoiceId)
    {
        var payments = _context.Payments
            .Where(p => p.InvoiceId == invoiceId)
            .Select(p => new
            {
                p.Id,
                p.Amount,
                p.PaymentDate
            })
            .ToList();

        return Ok(payments);
    }
}