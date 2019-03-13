using Microsoft.EntityFrameworkCore;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public class CustomerAPIContext : DbContext
    {
        public CustomerAPIContext(DbContextOptions<CustomerAPIContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
