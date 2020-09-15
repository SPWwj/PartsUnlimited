using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartsUnlimited.Models;

namespace PartsUnlimited.OrderTracking
{
    public class MockOrdersClient : IOrdersClient
    {
        static Order MockOrder = new Order
        {
            OrderId = 123,
            OrderDate = DateTime.Now,
            Name = "Daniel Roth",
            Address = "One Microsoft Way",
            City = "Redmond",
            State = "WA",
            PostalCode = "98052",
            Country = "USA",
            Email = "daroth@microsoft.com",
            Username = "danroth27"
        };

        public Task<OrderWithStatus> GetOrder(int orderId)
        {
            if (MockOrder?.OrderId != orderId)
            {
                MockOrder = new Order
                {
                    OrderId = orderId,
                    OrderDate = DateTime.Now,
                    Name = "Daniel Roth",
                    Address = "One Microsoft Way",
                    City = "Redmond",
                    State = "WA",
                    PostalCode = "98052",
                    Country = "USA",
                    Email = "daroth@microsoft.com",
                    Username = "danroth27"
                };
            }
            return Task.FromResult(OrderWithStatus.FromOrder(MockOrder));
        }
    }
}
