namespace SmartInvoice.API.DTOs;

public class CreateInvoiceDto
{
    public int ClientId { get; set; }
    public DateTime DueDate { get; set; }
    public List<InvoiceItemDto> Items { get; set; }
}