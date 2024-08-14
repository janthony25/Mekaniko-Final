using Mekaniko_Final.Data;
using Mekaniko_Final.Models.Dto;
using Mekaniko_Final.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mekaniko_Final.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _data;
        public CustomerRepository(DataContext data)
        {
            _data = data;
        }

        public async Task<CustomerDto> GetCustomerCarByCustomerIdAsync(int id)
        {
            return await _data.Customers
                .Include(c => c.Car)
                    .ThenInclude(car => car.CarMake)
                        .ThenInclude(cm => cm.Make)
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerEmail = c.CustomerEmail,
                    CustomerNumber = c.CustomerNumber,
                    Car = c.Car.Select(car => new CarDto
                    {
                        CarId = car.CarId,
                        CarRego = car.CarRego,
                        CarModel = car.CarModel,
                        CarYear = car.CarYear,
                        CarPaymentStatus = car.CarPaymentStatus,
                        MakeName = car.CarMake.Select(cm => new MakeDto
                        {
                            MakeId = cm.Make.MakeId,
                            MakeName = cm.Make.MakeName
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<List<CustomerSummaryDto>> GetCustomerListAsync()
        {
            return await _data.Customers
                .Select(c => new CustomerSummaryDto
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerEmail = c.CustomerEmail,
                    CustomerNumber = c.CustomerNumber
                }).ToListAsync();
        }
    }
}
