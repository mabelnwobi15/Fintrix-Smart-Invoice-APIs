using Microsoft.EntityFrameworkCore;
using SmartInvoice.API.Models;

namespace SmartInvoice.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>()
            .Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InvoiceItem>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }
}