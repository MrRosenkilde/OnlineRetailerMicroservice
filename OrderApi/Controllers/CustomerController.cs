using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{ 
    [Route("/api/customers")]
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> customers;
        public CustomerController(IRepository<Customer> customers)
        {
            this.customers = customers;
        }
        [HttpGet]
        public IActionResult Get() => new ObjectResult ( customers.GetAll() );
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var customer = customers.Get(id);
            if (customer == null) return NotFound();
            else return new ObjectResult(customer);
        }
        [HttpPost()]
        public IActionResult Post([FromBody]Customer c) {
            var customer = customers.Get(c.Id);
            if (customer == null) {
                customers.Add(c);
                return Ok();
            }
            else customers.Edit(c);
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete([FromBody]Customer c) {
            var customer = customers.Get(c.Id);
            if (c == null)
                return BadRequest();
            else customers.Remove(c.Id);
            return Ok();
        }

    }
}
