using PartsUnlimited.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PartsUnlimited.Shared
{
    public class CartClient
    {
        public CartClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<IList<CartSummary>> GetCartSummary()
        {
            return await Client.GetFromJsonAsync<CartSummary[]>("api/ShoppingCart/Summary");
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
