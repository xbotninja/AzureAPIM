using Contoso.Customers.Models;
using Contoso.Customers.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Customers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
       
        private readonly ICustomerService _customers;

        public CustomersController(ICustomerService customers)
        {
            _customers = customers;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customers.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Customer> GetById(int id)
        {
            return await _customers.GetById(id);
        }

        [HttpPut]
        public async Task<Customer> Update(Customer customer)
        {
            return await _customers.Update(customer);
        }

        [HttpPost]
        public async Task<int> Create(int id, Customer customer)
        {
            return await _customers.Create(customer);
        }

    }
}
