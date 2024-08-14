using Mekaniko_Final.Models.Dto;

namespace Mekaniko_Final.Repository.IRepository
{
    public interface ICustomerRepository
    {
        Task<List<CustomerSummaryDto>> GetCustomerListAsync();
        Task<CustomerDto> GetCustomerCarByCustomerIdAsync(int id);
    }
}
