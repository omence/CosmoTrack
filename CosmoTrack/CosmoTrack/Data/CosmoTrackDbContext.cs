using CosmoTrack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Data
{
    public class CosmoTrackDbContext : DbContext
    {
        public CosmoTrackDbContext(DbContextOptions<CosmoTrackDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Review> Reviews { get; set; }
    }
}
