using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OrderApi.Models;

using System;
using System.Linq.Expressions;

namespace OrderApi.Data
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly OrderApiContext db;

        public OrderRepository(OrderApiContext context)
        {
            db = context;
        }
        public IEnumerable<Order> Search(Expression<Func<Order, bool>> predicate) => db.Orders.Where(predicate);
        public void Edit(Order entity)
        {
            db.Orders.Where(x => x.Id == entity.Id).ForEachAsync(x => x = entity).RunSynchronously();
            db.SaveChanges();
        }
        public Order Add(Order entity)
        {
            if (entity.Date == null)
                entity.Date = DateTime.Now;
            
            var newOrder = db.Orders.Add(entity).Entity;
            db.SaveChanges();
            return newOrder;
        }

        public void EditAsync(Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Order Get(int id)
        {
            return db.Orders.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Orders.ToList();
        }

        public void Remove(int id)
        {
            var order = db.Orders.FirstOrDefault(p => p.Id == id);
            db.Orders.Remove(order);
            db.SaveChanges();
        }
        public void ChangeStatus(OrderStatus status, int orderId)
        {
            db.Orders.FirstOrDefault(x => x.Id == orderId).Status = status;
            db.SaveChanges();
        }

    }
}
