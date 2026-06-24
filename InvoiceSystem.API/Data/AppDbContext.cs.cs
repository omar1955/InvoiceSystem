using InvoiceSystem.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Store 1 : Many Invoices
            builder.Entity<Invoice>()
                .HasOne(i => i.Store)
                .WithMany(s => s.Invoices)
                .HasForeignKey(i => i.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer 1 : Many Invoices
            builder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice 1 : Many InvoiceItems
            builder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product 1 : Many InvoiceItems
            builder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Product)
                .WithMany(p => p.InvoiceItems)
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Customer>()
            .HasOne(c => c.ApplicationUser)
          .WithMany()
          .HasForeignKey(c => c.ApplicationUserId)
           .OnDelete(DeleteBehavior.SetNull);

            // Decimal precision
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<Invoice>()
                .Property(i => i.TotalPrice)
                .HasPrecision(18, 2);

            builder.Entity<InvoiceItem>()
                .Property(ii => ii.Price)
                .HasPrecision(18, 2);

            builder.Entity<InvoiceItem>()
                .Property(ii => ii.DiscountPercentage)
                .HasPrecision(5, 2);

            builder.Entity<InvoiceItem>()
                .Property(ii => ii.TaxPercentage)
                .HasPrecision(5, 2);

            builder.Entity<InvoiceItem>()
                .Property(ii => ii.Total)
                .HasPrecision(18, 2);

            // Serial should be unique
            builder.Entity<Invoice>()
                .HasIndex(i => i.Serial)
                .IsUnique();

            // Seed Data for dropdowns
            builder.Entity<Store>().HasData(
                new Store { Id = 1, Name = "Alexandria Store", Address = "Alexandria" },
                new Store { Id = 2, Name = "Cairo Store", Address = "Cairo" }
            );

            builder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Ahmed Ali", Email = "ahmed@test.com", Phone = "01000000001" },
                new Customer { Id = 2, Name = "Omar Mohamed", Email = "omar@test.com", Phone = "01000000002" }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 30000 },
                new Product { Id = 2, Name = "Mouse", Price = 500 },
                new Product { Id = 3, Name = "Keyboard", Price = 1000 }
            );

            builder.Entity<Customer>()
    .HasIndex(c => c.Name);

            builder.Entity<Store>()
                .HasIndex(s => s.Name);

            builder.Entity<Invoice>()
                .HasIndex(i => i.CreatedAt);
        }
    }
}