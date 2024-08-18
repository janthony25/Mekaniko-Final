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

        public async Task<InvoiceDetailsDto> GetInvoiceDetailsByIdAsync(int id)
        {
            var invoice = await _data.Invoices
                .Include(i => i.Car)
                    .ThenInclude(car => car.Customer)
                .Include(i => i.Car)
                    .ThenInclude(car => car.CarMake)
                        .ThenInclude(cm => cm.Make)
                .Include(i => i.InvoiceItem)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if(invoice == null)
            {
                throw new ArgumentException("Invalid InvoiceId");
            }

            var invoiceDetails = new InvoiceDetailsDto
            {
                InvoiceId = invoice.InvoiceId,
                CarId = invoice.Car.CarId,
                CustomerName = invoice.Car.Customer.CustomerName,
                CustomerEmail = invoice.Car.Customer.CustomerEmail,
                CustomerNumber = invoice.Car.Customer.CustomerNumber,
                CarRego = invoice.Car.CarRego,
                MakeId = invoice.Car.CarMake.Select(cm => cm.Make.MakeId).FirstOrDefault(),
                MakeName = invoice.Car.CarMake.Select(cm => cm.Make.MakeName).FirstOrDefault(),
                CarModel = invoice.Car.CarModel,
                CarYear = invoice.Car.CarYear,
                DateAdded = invoice.DateAdded,
                DueDate = invoice.DueDate,
                IssueName = invoice.IssueName,
                PaymentTerm = invoice.PaymentTerm,
                Notes = invoice.Notes,
                LaborPrice = invoice.LaborPrice,
                Discount = invoice.Discount,
                ShippingFee = invoice.ShippingFee,
                SubTotal = invoice.SubTotal,
                IsPaid = invoice.IsPaid,
                TotalAmount = invoice.TotalAmount,
                AmountPaid = invoice.AmountPaid,
                InvoiceItems = invoice.InvoiceItem.Select(item => new UpdateInvoiceItemDto
                {
                    InvoiceItemId = item.InvoiceItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    ItemPrice = item.ItemPrice,
                    ItemTotal = item.ItemTotal
                }).ToList()
            };

            return invoiceDetails;
        }

        public async Task UpdateInvoiceAsync(UpdateCarInvoiceDto dto)
        {
            var invoice = await _data.Invoices
                .Include(i => i.InvoiceItem)
                .FirstOrDefaultAsync(i => i.InvoiceId == dto.InvoiceId);

            if (invoice == null)
            {
                throw new ArgumentException("Invalid InvoiceId");
            }

            // Update invoice details
            invoice.DateAdded = dto.DateAdded;
            invoice.DueDate = dto.DueDate;
            invoice.IssueName = dto.IssueName;
            invoice.PaymentTerm = dto.PaymentTerm;
            invoice.Notes = dto.Notes;
            invoice.LaborPrice = dto.LaborPrice;
            invoice.Discount = dto.Discount;
            invoice.ShippingFee = dto.ShippingFee;
            invoice.SubTotal = dto.SubTotal;
            invoice.TotalAmount = dto.TotalAmount;
            invoice.AmountPaid = dto.AmountPaid;
            invoice.IsPaid = dto.IsPaid;

            // Update existing items and add new items
            foreach (var itemDto in dto.InvoiceItems)
            {
                if (itemDto.InvoiceItemId > 0)
                {
                    // Update existing item
                    var existingItem = invoice.InvoiceItem
                        .FirstOrDefault(ii => ii.InvoiceItemId == itemDto.InvoiceItemId);

                    if (existingItem != null)
                    {
                        existingItem.ItemName = itemDto.ItemName;
                        existingItem.Quantity = itemDto.Quantity;
                        existingItem.ItemPrice = itemDto.ItemPrice;
                        existingItem.ItemTotal = itemDto.ItemTotal;
                    }
                }
                else
                {
                    // Add new item
                    var newItem = new InvoiceItem
                    {
                        ItemName = itemDto.ItemName,
                        Quantity = itemDto.Quantity,
                        ItemPrice = itemDto.ItemPrice,
                        ItemTotal = itemDto.ItemTotal,
                        InvoiceId = invoice.InvoiceId // Set the correct InvoiceId
                    };
                    _data.InvoiceItems.Add(newItem);
                }
            }

            // Save changes to the database
            await _data.SaveChangesAsync();
        }


    }
}
