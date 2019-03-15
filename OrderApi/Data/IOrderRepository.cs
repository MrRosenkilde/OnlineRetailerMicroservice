using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public interface IOrderRepository<T> : IRepository<T> 
    {
        void SetOrderPaid(int orderId);
        void SetOrderCancelled(int orderId);
        void SetOrderShip(int orderId);

    }
}
