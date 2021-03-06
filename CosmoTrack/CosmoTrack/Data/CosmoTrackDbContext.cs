﻿using CosmoTrack.Models;
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

        public DbSet<Follow> Follows { get; set; }

        public DbSet<UserJournal> UserJournals { get; set; }

        public DbSet<ProductJournal> ProductJournals { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Image> Images { get; set; }

        [DbFunction(FunctionName = "SOUNDEX", Schema = "")]
        public static string SoundsLike(string SearchString)
        {
            throw new NotImplementedException();
        }
    }

}
