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
    public class UpdateUnpaidBills : IHostedService
    {
        private IRepository<Order> Orders;
        private IDisposable disp;
        private readonly TaskCompletionSource<Task> tcs;
        public UpdateUnpaidBills(IRepository<Order> orders)
        {
            Orders = orders;
            tcs = new TaskCompletionSource<Task>();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            tcs.SetResult( Task.Factory.StartNew(Job) );
            return tcs.Task.Result;
        }
        private void Job()
        {
            foreach (Order o in Orders.Search(x => x.Status == OrderStatus.SHIPPED && x.Date.Value.AddMonths(1).CompareTo(DateTime.Now) <= 0).AsEnumerable())
                Orders.Edit(new Order() {
                    CustomerId = o.CustomerId,
                    Date = o.Date,
                    Status = OrderStatus.UNPAID,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    Id = o.Id
                });

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return tcs.Task;
        }
    }
}
