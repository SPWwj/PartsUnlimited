using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PartsUnlimited.Shared
{
    public class CartClient : PublicCartClient, ICartClient
    {
        public CartClient(HttpClient client) : base(client)
        {
        }

        public async Task<ShoppingCartModel> GetCartDetails()
        {
            return await Client.GetFromJsonAsync<ShoppingCartModel>("api/ShoppingCart");
        }

        public async Task RemoveCartItem(int itemId)
        {
            await Client.DeleteAsync($"api/ShoppingCart/Remove/{itemId}");
        }
    }

    public interface ICartClient
    {
        Task<ShoppingCartModel> GetCartDetails();
        Task RemoveCartItem(int itemId);
    }
}
