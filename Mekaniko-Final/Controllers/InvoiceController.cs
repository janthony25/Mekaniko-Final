using Mekaniko_Final.Models.Dto;
using Mekaniko_Final.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Mekaniko_Final.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceController(IInvoiceRepository invoiceRepository, ICarRepository carRepository)
        {
            _invoiceRepository = invoiceRepository;
            _carRepository = carRepository;
        }
        

        // GET - ADD Invoice to Car
        public async Task<IActionResult> AddInvoiceToCar(int id)
        {
            var carDetails = await _carRepository.GetCustomerCarSummaryByCarIdAsync(id);

            var model = new AddInvoiceToCarDto
            {
                CustomerName = carDetails.CustomerName,
                CustomerEmail = carDetails.CustomerEmail,
                CustomerNumber = carDetails.CustomerNumber,
                CarId = carDetails.CarId,
                CarRego = carDetails.CarRego,
                MakeName = carDetails.MakeName,
                CarModel = carDetails.CarModel,
                CarYear = carDetails.CarYear,
                IssueName = string.Empty,
                InvoiceItems = new List<AddInvoiceItemDto>()
            };

            return View(model);
        }

        // POST - Add Invoice to Car
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInvoiceToCar(AddInvoiceToCarDto dto)
        {
            if (ModelState.IsValid)
            {
                await _invoiceRepository.AddInvoiceToCarAsync(dto);
                return RedirectToAction("GetCarInvoiceSummary", "Car", new { id = dto.CarId });
            }
            return View(dto);
        }

        // GET - Update Invoice Details
        public async Task<IActionResult> UpdateInvoiceDetails(int id)
        {
            var invoiceDetails = await _invoiceRepository.GetInvoiceDetailsByIdAsync(id);

            if (invoiceDetails == null)
            {
                return NotFound();
            }

            var model = new UpdateCarInvoiceDto
            {
                InvoiceId = invoiceDetails.InvoiceId,
                CustomerName = invoiceDetails.CustomerName,
                CustomerEmail = invoiceDetails.CustomerEmail,
                CustomerNumber = invoiceDetails.CustomerNumber,
                CarId = invoiceDetails.CarId,
                CarRego = invoiceDetails.CarRego,
                MakeId = invoiceDetails.MakeId,
                MakeName = invoiceDetails.MakeName,
                CarModel = invoiceDetails.CarModel,
                CarYear = invoiceDetails.CarYear,
                DateAdded = invoiceDetails.DateAdded,
                DueDate = invoiceDetails.DueDate,
                IssueName = invoiceDetails.IssueName,
                PaymentTerm = invoiceDetails.PaymentTerm,
                Notes = invoiceDetails.Notes,
                LaborPrice = invoiceDetails.LaborPrice,
                Discount = invoiceDetails.Discount,
                ShippingFee = invoiceDetails.ShippingFee,
                SubTotal = invoiceDetails.SubTotal,
                TotalAmount = invoiceDetails.TotalAmount,
                AmountPaid = invoiceDetails.AmountPaid,
                IsPaid = invoiceDetails.IsPaid,
                InvoiceItems = invoiceDetails.InvoiceItems.Select(item => new UpdateInvoiceItemDto
                {
                    InvoiceItemId = item.InvoiceItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    ItemPrice = item.ItemPrice,
                    ItemTotal = item.ItemTotal
                }).ToList()
            };

            return View(model);
        }
    }
}
