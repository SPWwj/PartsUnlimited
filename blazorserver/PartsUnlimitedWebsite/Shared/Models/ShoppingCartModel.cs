using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartsUnlimited.Shared
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
        }

        public List<CartItemModel> CartItems { get; set; }
        public int CartCount { get; set; }
        public OrderCostSummary OrderCostSummary { get; set; }
    }

    public class CartItemModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public ProductModel Product { get; set; }
    }

    public class ProductModel
    {
        public string ProductArtUrl { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
