// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PartsUnlimited.Models;
using System;
using System.Collections.Generic;

namespace PartsUnlimited.ViewModels
{
    public class OrdersModel
    {
        public bool IsAdminSearch { get; init; }
        public string InvalidOrderSearch { get; init; }
        public IEnumerable<Order> Orders { get; init; }
        public string Username { get; init;  }
        public DateTimeOffset StartDate { get; init; }
        public DateTimeOffset EndDate { get; init; }

        public OrdersModel(IEnumerable<Order> orders, string username, DateTimeOffset startDate, DateTimeOffset endDate, string invalidOrderSearch, bool isAdminSearch)
        {
            Orders = orders;
            Username = username;
            StartDate = startDate;
            EndDate = endDate;
            InvalidOrderSearch = invalidOrderSearch;
            IsAdminSearch = isAdminSearch;
        }
    }
}