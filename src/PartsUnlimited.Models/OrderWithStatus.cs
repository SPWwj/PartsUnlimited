using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartsUnlimited.Models;

namespace PartsUnlimited.Models
{
    public class OrderWithStatus
    {
        public Order Order { get; set; }

        public IList<StatusUpdate> StatusUpdates { get; set; } = new List<StatusUpdate>();

        public bool IsDelivered => StatusUpdates.Any(update => update.StatusText == "Delivered");

        public DateTime ExpectedDeliveryDate { get; set; }

        public class StatusUpdate
        {
            public DateTime TimeStamp { get; set; }
            public string StatusText { get; set; }
        }

        public static OrderWithStatus FromOrder(Order order)
        {
            var orderWithStatus = new OrderWithStatus
            {
                Order = order,
                ExpectedDeliveryDate = order.OrderDate.Add(TimeSpan.FromSeconds(30))
            };

            // To simulate a real backend process, we fake status updates based on the amount
            // of time since the order was placed
            var shippedTime = order.OrderDate.Add(TimeSpan.FromSeconds(10));
            var outForDeliveryTime = shippedTime.Add(TimeSpan.FromSeconds(10));

            if (DateTime.Now > shippedTime)
            {
                orderWithStatus.StatusUpdates.Add(new StatusUpdate { StatusText = "Shipped", TimeStamp = shippedTime });
            }
            if (DateTime.Now > outForDeliveryTime)
            {
                orderWithStatus.StatusUpdates.Add(new StatusUpdate { StatusText = "Out for delivery", TimeStamp = outForDeliveryTime });
            }
            if (DateTime.Now > orderWithStatus.ExpectedDeliveryDate)
            {
                orderWithStatus.StatusUpdates.Add(new StatusUpdate { StatusText = "Delivered", TimeStamp = orderWithStatus.ExpectedDeliveryDate });
            }

            return orderWithStatus;
        }
    }
}
