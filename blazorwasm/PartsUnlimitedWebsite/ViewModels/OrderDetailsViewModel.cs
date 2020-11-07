// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PartsUnlimited.Models;
using PartsUnlimited.Shared;

namespace PartsUnlimited.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderCostSummary OrderCostSummary { get; set; }
        public Order Order { get; set; }
    }
}