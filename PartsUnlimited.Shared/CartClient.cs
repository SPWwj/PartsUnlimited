using PartsUnlimited.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PartsUnlimited.Shared
{
    public class PublicCartClient
    {
        public PublicCartClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<IList<CartSummary>> GetCartSummary()
        {
            return await Client.GetFromJsonAsync<CartSummary[]>("api/ShoppingCart/Summary");
        }
    }

    public class CartClient : PublicCartClient
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
}
