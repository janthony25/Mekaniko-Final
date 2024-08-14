using Mekaniko_Final.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Mekaniko_Final.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository _carRepository;
        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        

        // GET - Car-Invoice by Id
        public async Task<IActionResult> GetCarInvoiceSummary(int id)
        {
            var carInvoice = await _carRepository.GetCarInvoiceSummaryByCarIdAsync(id);
            return View(carInvoice);
        }
    }
}
