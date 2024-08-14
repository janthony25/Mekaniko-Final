using Mekaniko_Final.Models.Dto;

namespace Mekaniko_Final.Repository.IRepository
{
    public interface ICarRepository
    {
        Task<List<GetCarInvoiceSummaryDto>> GetCarInvoiceSummaryByCarIdAsync(int id);
        Task<GetCustomerCarByCarIdDto> GetCustomerCarSummaryByCarIdAsync(int id);
    }
}
