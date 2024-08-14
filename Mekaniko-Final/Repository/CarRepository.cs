using Mekaniko_Final.Data;
using Mekaniko_Final.Models;
using Mekaniko_Final.Models.Dto;
using Mekaniko_Final.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mekaniko_Final.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly DataContext _data;
        public CarRepository(DataContext data)
        {
            _data = data;
        }
        public async Task<List<GetCarInvoiceSummaryDto>> GetCarInvoiceSummaryByCarIdAsync(int id)
        {
            return await _data.Cars
                .Include(car => car.Customer)
                .Include(car => car.CarMake)
                    .ThenInclude(cm => cm.Make)
                .Include(car => car.Invoice)
                .Where(car => car.CarId == id)
                .SelectMany(car => car.Invoice.DefaultIfEmpty(), (car, invoice) => new GetCarInvoiceSummaryDto
                {
                    CustomerId = car.Customer.CustomerId,
                    CustomerName = car.Customer.CustomerName,
                    CustomerEmail = car.Customer.CustomerEmail,
                    CustomerNumber = car.Customer.CustomerNumber,
                    CarId = car.CarId,
                    CarRego = car.CarRego,
                    MakeName = car.CarMake.FirstOrDefault().Make.MakeName,
                    CarModel = car.CarModel,
                    CarYear = car.CarYear,
                    CarPaymentStatus = car.CarPaymentStatus,
                    InvoiceId = invoice != null ? invoice.InvoiceId : 0,
                    DateAdded = invoice.DateAdded,
                    DueDate = invoice.DueDate,
                    IssueName = invoice.IssueName,
                    TotalAmount = invoice.TotalAmount,
                    AmountPaid = invoice.AmountPaid,
                    IsPaid = invoice.IsPaid
                }).ToListAsync();
        }

        public async Task<GetCustomerCarByCarIdDto> GetCustomerCarSummaryByCarIdAsync(int id)
        {


            return await _data.Cars
                .Include(car => car.Customer)
                 .Include(car => car.CarMake)
                    .ThenInclude(cm => cm.Make)
                .Where(car => car.CarId == id)
                .Select(car => new GetCustomerCarByCarIdDto
                {
                    CustomerId = car.Customer.CustomerId,
                    CustomerName = car.Customer.CustomerName,
                    CustomerEmail = car.Customer.CustomerEmail,
                    CustomerNumber = car.Customer.CustomerNumber,
                    CarId = car.CarId,
                    CarRego = car.CarRego,
                    MakeName = car.CarMake.Select(cm => cm.Make.MakeName).FirstOrDefault(),
                    CarModel = car.CarModel,
                    CarYear = car.CarYear
                }).FirstOrDefaultAsync();
        }
    }
}
