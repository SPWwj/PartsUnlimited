using Microsoft.EntityFrameworkCore;
using PartsUnlimited.Models;
using PartsUnlimited.Shared;
using PartsUnlimited.Shared.Models;
using PartsUnlimited.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimitedWebsite.Services
{
    public class ShoppingCartService
    {
        private readonly IPartsUnlimitedContext _context;

        public ShoppingCartService(IPartsUnlimitedContext context)
        {
            _context = context;
        }

        public async Task RemoveFromCart(string shoppingCartId, int id)
        {
            var cart = new ShoppingCart(_context, shoppingCartId);

            cart.RemoveFromCart(id);

            await _context.SaveChangesAsync(default);
        }

        internal async Task<ShoppingCartModel> GetShoppingCartDetails(string shoppingCartId)
        {
            var cart = new ShoppingCart(_context, shoppingCartId);

            var items = await cart.GetCartItemsAsync();

            var itemsCount = items.Sum(x => x.Count);
            var subTotal = items.Sum(x => x.Count * x.Product.Price);
            var shipping = itemsCount * (decimal)5.00;
            var tax = (subTotal + shipping) * (decimal)0.05;
            var total = subTotal + shipping + tax;

            var costSummary = new OrderCostSummary
            {
                CartSubTotal = subTotal.ToString("C"),
                CartShipping = shipping.ToString("C"),
                CartTax = tax.ToString("C"),
                CartTotal = total.ToString("C")
            };


            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = items,
                CartCount = itemsCount,
                OrderCostSummary = costSummary
            };

            return ShoppingCartViewModel.ToModel(viewModel);
        }

        public async Task<IList<CartSummary>> GetShoppingCartSummary(string shoppingCartId)
        {
            return await _context.CartItems.Where(cart => cart.CartId == shoppingCartId)
                .Select(a => new CartSummary { Title = a.Product.Title, Count = a.Count })
                .OrderBy(x => x.Title)
                .ToListAsync();
        }
    }
}
