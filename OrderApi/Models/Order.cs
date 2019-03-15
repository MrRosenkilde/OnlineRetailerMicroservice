using System;
using System.Collections.Generic;

namespace OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
        public IList<OrderLine> OrderLInes { get; set; }
    }
    public enum OrderStatus
    {
        COMPLETED,CANCELLED,SHIPPED,PAID,UNPAID
    }
    public class OrderLine {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
