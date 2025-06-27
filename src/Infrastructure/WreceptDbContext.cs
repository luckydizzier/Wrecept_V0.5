using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Domain;

namespace Wrecept.Infrastructure;

public class WreceptDbContext : DbContext
{
    public WreceptDbContext(DbContextOptions<WreceptDbContext> options)
        : base(options)
    {
    }

    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>(e =>
        {
            e.ToTable("Suppliers");
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).IsRequired();
            e.Property(s => s.Address).IsRequired();
            e.Property(s => s.TaxId).IsRequired();
            e.Property(s => s.BankAccountNumber).IsRequired();
        });

        modelBuilder.Entity<ProductGroup>(e =>
        {
            e.ToTable("ProductGroups");
            e.HasKey(pg => pg.Id);
            e.Property(pg => pg.Name).IsRequired();
        });

        modelBuilder.Entity<TaxRate>(e =>
        {
            e.ToTable("TaxRates");
            e.HasKey(tr => tr.Id);
            e.Property(tr => tr.Label).IsRequired();
            e.Property(tr => tr.Percentage).IsRequired();
        });

        modelBuilder.Entity<Unit>(e =>
        {
            e.ToTable("Units");
            e.HasKey(u => u.Id);
            e.Property(u => u.Name).IsRequired();
            e.Property(u => u.Symbol).IsRequired();
        });

        modelBuilder.Entity<PaymentMethod>(e =>
        {
            e.ToTable("PaymentMethods");
            e.HasKey(pm => pm.Id);
            e.Property(pm => pm.Label).IsRequired();
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasKey(p => p.Id);
            e.Property<Guid>("ProductGroupId");
            e.Property<Guid>("TaxRateId");
            e.Property<Guid>("DefaultUnitId");
            e.HasOne(p => p.Group).WithMany().HasForeignKey("ProductGroupId");
            e.HasOne(p => p.TaxRate).WithMany().HasForeignKey("TaxRateId");
            e.HasOne(p => p.DefaultUnit).WithMany().HasForeignKey("DefaultUnitId");
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.ToTable("Invoices");
            e.HasKey(i => i.Id);
            e.Property(i => i.SerialNumber).IsRequired();
            e.Property(i => i.CalculationMode).IsRequired();
            e.Property<Guid>("SupplierId");
            e.Property<Guid>("PaymentMethodId");
            e.HasOne(i => i.Supplier).WithMany().HasForeignKey("SupplierId");
            e.HasOne(i => i.PaymentMethod).WithMany().HasForeignKey("PaymentMethodId");
        });

        modelBuilder.Entity<InvoiceItem>(e =>
        {
            e.ToTable("InvoiceItems");
            e.HasKey(ii => ii.Id);
            e.Property<Guid>("InvoiceId");
            e.Property<Guid>("ProductId");
            e.Property<Guid>("UnitId");
            e.HasOne<Invoice>().WithMany(i => i.Items).HasForeignKey("InvoiceId");
            e.HasOne(ii => ii.Product).WithMany().HasForeignKey("ProductId");
            e.HasOne(ii => ii.Unit).WithMany().HasForeignKey("UnitId");
        });
    }
}
