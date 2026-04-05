namespace SmartInvoice.API.DTOs;

public class CreatePaymentDto
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
}