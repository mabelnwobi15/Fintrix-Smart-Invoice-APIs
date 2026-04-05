namespace SmartInvoice.API.DTOs;

public class InvoiceItemDto
{
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}