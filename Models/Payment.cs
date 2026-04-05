namespace SmartInvoice.API.Models;

public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }

    // Add this
    public Invoice Invoice { get; set; } 
}