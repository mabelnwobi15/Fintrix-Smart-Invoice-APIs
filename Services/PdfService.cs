using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SmartInvoice.API.Data;

namespace SmartInvoice.API.Services;

public class PdfService
{
    private readonly AppDbContext _context;

    public PdfService(AppDbContext context)
    {
        _context = context;
    }

    public byte[] GenerateInvoicePdf(int invoiceId)
    {
        var invoice = _context.Invoices.FirstOrDefault(i => i.Id == invoiceId);
        var client = _context.Clients.FirstOrDefault(c => c.Id == invoice.ClientId);
        var items = _context.InvoiceItems.Where(i => i.InvoiceId == invoiceId).ToList();

        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph("INVOICE").SetFontSize(20));

        document.Add(new Paragraph($"Client: {client.Name}"));
        document.Add(new Paragraph($"Email: {client.Email}"));

        document.Add(new Paragraph($"Date: {invoice.Date}"));
        document.Add(new Paragraph($"Due Date: {invoice.DueDate}"));
        document.Add(new Paragraph($"Status: {invoice.Status}"));

        document.Add(new Paragraph(" "));

        var table = new Table(3);
        table.AddHeaderCell("Description");
        table.AddHeaderCell("Qty");
        table.AddHeaderCell("Price");

        foreach (var item in items)
        {
            table.AddCell(item.Description);
            table.AddCell(item.Quantity.ToString());
            table.AddCell(item.Price.ToString("C"));
        }

        document.Add(table);

        document.Add(new Paragraph($"Total: {invoice.TotalAmount:C}"));

        document.Close();

        return ms.ToArray();
    }
}