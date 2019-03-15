using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using RestSharp;

namespace OrderApi.Controllers
{
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository<Order> orderRepo;
        private readonly IRepository<Customer> customers;
        public OrdersController(IOrderRepository<Order> repos, IRepository<Customer> customers)
        {
            orderRepo = repos;
            this.customers = customers;
        }

        // GET: api/orders
        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(orderRepo.GetAll());
        }
        // GET api/Order/
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = orderRepo.Get(id);

            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/orders
        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            var customer = customers.Get(order.CustomerId);
            if (customer == null) return NotFound("Customer not found");
            if (customer.CreditStanding == CreditStanding.HORRIBLE || customer.CreditStanding == CreditStanding.BAD)
                return BadRequest("Customer credit too low");
            // Call ProductApi to get the product ordered
            RestClient c = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            c.BaseUrl = new Uri("https://localhost:44311/api/products/");
            var request = new RestRequest(order.ProductId.ToString(), Method.GET);
            var response = c.Execute<Product>(request);
            var orderedProduct = response.Data;

            if (order.Quantity <= orderedProduct.ItemsInStock)
            {
                // reduce the number of items in stock for the ordered product,
                // and create a new order.
                orderedProduct.ItemsReserved += order.Quantity;
                var updateRequest = new RestRequest(orderedProduct.Id.ToString(), Method.PUT);
                updateRequest.AddJsonBody(orderedProduct);
                var updateResponse = c.Execute(updateRequest);

                if (updateResponse.IsSuccessful)
                {
                    order.Status = OrderStatus.SHIPPED;
                    var newOrder = orderRepo.Add(order);
                    return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
                }
            }
            else return BadRequest("Not enough items in stock");
            // If the order could not be created, "return no content".
            return NoContent();
        }
        [HttpPut("{id}/changeOrderStatus/cancel")]
        public IActionResult CancelOrder(int id)
        {
            orderRepo.SetOrderCancelled(id);
            return Ok();
        }
        [HttpPut("{id}/changeOrderStatus/pay")]
        public IActionResult Pay(int id)
        {
            var order = orderRepo.Get(id);
            if (order == null)
                return new NotFoundObjectResult("couldn't find order with id : " + id);
            orderRepo.SetOrderPaid(id);
            return new ObjectResult(orderRepo.Get(id));
        }
        [HttpPut("{id}/changeOrderStatus/ship")]
        public IActionResult Ship(int id)
        {
            orderRepo.SetOrderShip(id);
            return Ok();
        }

    }
}
