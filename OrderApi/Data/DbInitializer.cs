using System.Collections.Generic;
using System.Linq;
using OrderApi.Models;
using System;

namespace OrderApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(OrderApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }

            List<Order> orders = new List<Order>
            {
                new Order {
                    Date = DateTime.Today,
                    ProductId = 1,
                    Quantity = 2,
                    CustomerId =0
                }
            };

            

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
        public void Initialize(CustomerAPIContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            List<Customer> customers = new List<Customer>
            {
                new Customer
                {
                    Id = 0,
                    Phone = "31575600",
                    Name = "Dennis Rosenkilde",
                    Email = "Dennis@Rosenkilde.nu",
                    BillingAddress= "Ribevej 62A 6760 Ribe",
                    ShippingAddress ="Ribevej 62A 6760 Ribe",
                    CreditStanding = CreditStanding.EXCELLENT
                },
                new Customer
                {
                    Phone="your mama",
                    Name = "That other guy",
                    Email = "fake@andgay.com",
                    BillingAddress= "Somewhere",
                    ShippingAddress ="To Dennis Rosenkilde",
                    CreditStanding = CreditStanding.HORRIBLE
                },
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
