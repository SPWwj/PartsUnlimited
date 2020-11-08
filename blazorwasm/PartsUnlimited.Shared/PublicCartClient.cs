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
}
