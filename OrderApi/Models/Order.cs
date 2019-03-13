﻿using System;
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
    }
    public enum OrderStatus
    {
        COMPLETED,CANCELLED,SHIPPED,PAID,UNPAID
    }
}
