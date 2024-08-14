using Mekaniko_Final.Data;
using Mekaniko_Final.Models;
using Mekaniko_Final.Models.Dto;
using Mekaniko_Final.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mekaniko_Final.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataContext _data;
        public InvoiceRepository(DataContext data)
        {
            _data = data;
        }
        public async Task AddInvoiceToCarAsync(AddInvoiceToCarDto dto)
        {
            // Get the carId
            var car = await _data.Cars
                .Where(car => car.CarId == dto.CarId)
                .FirstOrDefaultAsync();

            // Add Invoice to Car
            var invoice = new Invoice
            {
                DateAdded = dto.DateAdded ?? DateTime.Now,
                DueDate = dto.DueDate,
                IssueName = dto.IssueName,
                PaymentTerm = dto.PaymentTerm,
                Notes = dto.Notes,
                LaborPrice = dto.LaborPrice,
                Discount = dto.Discount,
                ShippingFee = dto.ShippingFee,
                SubTotal = dto.SubTotal,
                TotalAmount = dto.TotalAmount,
                AmountPaid = dto.AmountPaid,
                IsPaid = dto.IsPaid,
                CarId = car.CarId
            };

            // Add invoice to Db
            _data.Invoices.Add(invoice);
            // Save changes to Db
            await _data.SaveChangesAsync();

          

            // Add InvoiceItem  
            var invoiceItem = dto.InvoiceItems.Select(item => new InvoiceItem
            {
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                ItemPrice = item.ItemPrice,
                ItemTotal = item.ItemTotal,
                InvoiceId = invoice.InvoiceId
            }).ToList();

            // Add Invoice to Db
            _data.InvoiceItems.AddRange(invoiceItem);
            // Save changes to Db
            await _data.SaveChangesAsync();

        }
    }
}
