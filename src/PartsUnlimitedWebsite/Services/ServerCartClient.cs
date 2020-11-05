using PartsUnlimited.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimitedWebsite.Services
{
    public class ServerCartClient : ICartClient
    {
        private readonly ShoppingCartService _service;

        public ServerCartClient(ShoppingCartService service)
        {
            _service = service;
        }

        public string ShoppingCartId { get; set; }

        public Task<ShoppingCartModel> GetCartDetails()
        {
            return _service.GetShoppingCartDetails(ShoppingCartId);
        }

        public Task RemoveCartItem(int itemId)
        {
            throw new InvalidOperationException("Can't remove items from the cart during prerendering.");
        }
    }
}
