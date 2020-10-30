// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using PartsUnlimited.Models;

namespace PartsUnlimited.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public int CartCount { get; set; }
        public OrderCostSummary OrderCostSummary { get; set; }
    }

    public class ShoppingCartViewModel2
    {
        public ShoppingCartViewModel2()
        {
        }

        public static ShoppingCartViewModel2 From(ShoppingCartViewModel source) => new ShoppingCartViewModel2(source);

        private ShoppingCartViewModel2(ShoppingCartViewModel source)
        {
            CartItems = source.CartItems.Select(ci => new CartItemModel 
                { 
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    Count = ci.Count,
                    Product = new ProductModel
                    {
                        Description = ci.Product.Description,
                        Price = ci.Product.Price,
                        ProductArtUrl = ci.Product.ProductArtUrl,
                        Title = ci.Product.ProductArtUrl
                    }
            }).ToList();

            CartCount = source.CartCount;
            OrderCostSummary = source.OrderCostSummary;
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
