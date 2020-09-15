using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartsUnlimited.Models
{
    public class PartsUnlimitedContext : DbContext
    {
        public PartsUnlimitedContext(DbContextOptions<PartsUnlimitedContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
