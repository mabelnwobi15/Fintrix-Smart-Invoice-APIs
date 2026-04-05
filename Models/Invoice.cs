namespace SmartInvoice.API.Models;

public class Invoice
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }

    public Client Client { get; set; }

    public ICollection<Payment> Payments { get; set; }
}
