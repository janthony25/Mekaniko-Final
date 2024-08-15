using Mekaniko_Final.Models.Dto;

namespace Mekaniko_Final.Repository.IRepository
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceToCarAsync(AddInvoiceToCarDto dto);
        Task<InvoiceDetailsDto> GetInvoiceDetailsByIdAsync(int id);
    }
}
