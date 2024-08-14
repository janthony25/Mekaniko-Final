using Mekaniko_Final.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Mekaniko_Final.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
       
        // GET - Customer List
        public async Task<IActionResult> GetCustomerList()
        {
            var customer = await _customerRepository.GetCustomerListAsync();
            return View(customer);
        }

        // GET - Customer Car by Id
        public async Task<IActionResult> GetCustomerCar(int id)
        {
            var customerCar = await _customerRepository.GetCustomerCarByCustomerIdAsync(id);
            return View(customerCar);
        }
    }
}
