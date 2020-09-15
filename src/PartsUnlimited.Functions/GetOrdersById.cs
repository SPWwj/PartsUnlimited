using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PartsUnlimited.Models;
using System.Linq;

namespace PartsUnlimited.Functions
{
    public class GetOrdersById
    {
        private readonly PartsUnlimitedContext context;

        public GetOrdersById(PartsUnlimitedContext context)
        {
            this.context = context;
        }

        [FunctionName("GetOrderById")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{id:int}")] HttpRequest req, int id, ILogger log)
        {
            var order = context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(OrderWithStatus.FromOrder(order));
        }
    }
}
