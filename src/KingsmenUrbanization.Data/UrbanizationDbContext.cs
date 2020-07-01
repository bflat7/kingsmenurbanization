using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Urbanization.Data.Abstractions;
using Urbanization.Data.Models;

namespace Urbanization.Data
{
    public class UrbanizationDbContext : DbContext, IUrbanizationDbContext
    {
        public UrbanizationDbContext(DbContextOptions<UrbanizationDbContext> options)
            : base(options) { }

        public DbSet<UrbanizationByState> UrbanizationByState { get; set; }
    }
}
