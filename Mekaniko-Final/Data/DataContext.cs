﻿using Mekaniko_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Mekaniko_Final.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<CarMake> CarMakes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1-to-M Customer-Car
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Car)
                .WithOne(car => car.Customer)
                .HasForeignKey(car => car.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // M-to-M Car-Make
            modelBuilder.Entity<CarMake>()
                .HasKey(cm => new { cm.CarId, cm.MakeId });

            modelBuilder.Entity<CarMake>()
                .HasOne(cm => cm.Car)
                .WithMany(car => car.CarMake)
                .HasForeignKey(cm => cm.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarMake>()
                .HasOne(cm => cm.Make)
                .WithMany(make => make.CarMake)
                .HasForeignKey(cm => cm.MakeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-M Car-Invoice
            modelBuilder.Entity<Car>()
                .HasMany(car => car.Invoice)
                .WithOne(i => i.Car)
                .HasForeignKey(i => i.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-M Invoice-InvoieItem
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceItem)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }

}
