using Microsoft.AspNetCore.Mvc;
using ABCRetailApp.Models;
using ABCRetailApp.Services;

namespace ABCRetailApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableStorageService<CustomerProfile> _tableStorageService;

        public CustomerController(TableStorageService<CustomerProfile> tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableStorageService.RetrieveAllEntitiesAsync();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerProfile customer)
        {
            if (ModelState.IsValid)
            {
                customer.PartitionKey = "CustomerProfile";
                customer.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.InsertOrMergeEntityAsync(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var customer = new CustomerProfile
            {
                PartitionKey = partitionKey,
                RowKey = rowKey
            };
            await _tableStorageService.DeleteEntityAsync(customer);
            return RedirectToAction("Index");
        }
    
}
}
