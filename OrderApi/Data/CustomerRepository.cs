using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerAPIContext db;
        public CustomerRepository(CustomerAPIContext customerContext) {
            this.db = customerContext;
        }
        public IEnumerable<Customer> GetAll() {
            return db.Customers.AsEnumerable<Customer>();
        }
        public Customer Get(int id) => db.Customers.FirstOrDefault(x => x.Id == id);
        public void Edit(Customer entity)
        {
            db.Customers.Where(x => x.Id == entity.Id).ForEachAsync(x => x = entity).RunSynchronously();
        }
        public void Remove(int id) => db.Remove( db.Customers.FirstOrDefault(x => x.Id == id) );
        public Customer Add(Customer entity) { db.Add(entity); db.SaveChanges();  return entity; }

        public IEnumerable<Customer> Search(Expression<Func<Customer, bool>> predicate) => db.Customers.Where(predicate);
    }
}
