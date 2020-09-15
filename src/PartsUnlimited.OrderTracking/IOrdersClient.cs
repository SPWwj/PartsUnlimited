using System.Threading.Tasks;
using PartsUnlimited.Models;

namespace PartsUnlimited.OrderTracking
{
    public interface IOrdersClient
    {
        Task<OrderWithStatus> GetOrder(int orderId);
    }
}