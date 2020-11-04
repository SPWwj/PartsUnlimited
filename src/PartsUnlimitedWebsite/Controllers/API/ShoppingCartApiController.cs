// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PartsUnlimited.Models;
using PartsUnlimited.Telemetry;
using PartsUnlimited.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PartsUnlimited.Shared;
using PartsUnlimitedWebsite.Services;
using System;

namespace PartsUnlimited.Controllers
{
    //[Authorize(JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ShoppingCartApiController : Controller
    {
        private readonly ShoppingCartService _cartService;
        private readonly ITelemetryProvider _telemetry;

        public ShoppingCartApiController(ShoppingCartService cartService, ITelemetryProvider telemetryProvider)
        {
            _cartService = cartService;
            _telemetry = telemetryProvider;
        }

        [HttpGet("api/ShoppingCart")]
        public async Task<IActionResult> Get()
        {
            var shoppingCartId = ShoppingCart.GetCartId(HttpContext);
            var cartModel = await _cartService.GetShoppingCartDetails(shoppingCartId);

            // Track cart review event with measurements
            _telemetry.TrackTrace("Cart/Server/Index");

            return Ok(cartModel);
        }

        [HttpGet("api/ShoppingCart/Summary")]
        public async Task<IActionResult> GetSummary()
        {
            var shoppingCartId = ShoppingCart.GetCartId(HttpContext);
            var cartSummary = await _cartService.GetShoppingCartSummary(shoppingCartId);

            return Ok(cartSummary);
        }

        [HttpDelete("api/ShoppingCart/Remove/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id, CancellationToken cancellationToken)
        {
            var cartId = ShoppingCart.GetCartId(HttpContext);

            try
            {
                await _cartService.RemoveFromCart(cartId, id);
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}