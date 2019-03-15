using Microsoft.Extensions.Hosting;
using OrderApi.Data;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderApi
{
    public class UpdateCreditStandingJob : IHostedService
    {
        private readonly IRepository<Customer> Customers;
        private readonly IRepository<Order> Orders;
        private IDisposable disp;
        public UpdateCreditStandingJob(IRepository<Customer> customerContext,IRepository<Order> orderContext)
        {
            Customers = customerContext;
            Orders = orderContext;
            
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            disp = Task.Factory.StartNew(Job);
            return Task.CompletedTask;
        }
        private Task Job()
        {
            foreach (Customer c in Customers.GetAll())
                if (Orders.Search(x => x.CustomerId == c.Id).Select(x => x.Status == OrderStatus.SHIPPED).Any())
                    c.CreditStanding = CreditStanding.HORRIBLE;
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            disp.Dispose();
            return Task.CompletedTask;
        }
    }
}
