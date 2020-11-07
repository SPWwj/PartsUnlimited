// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using PartsUnlimited.Models;
using PartsUnlimited.Shared;

namespace PartsUnlimited.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public int CartCount { get; set; }
        public OrderCostSummary OrderCostSummary { get; set; }

        public static ShoppingCartModel ToModel(ShoppingCartViewModel source)
        {
            return new ShoppingCartModel
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
                        Title = ci.Product.Title
                    }
                }).ToList(),
                CartCount = source.CartCount,
                OrderCostSummary = source.OrderCostSummary
            };
        }
    }
}
