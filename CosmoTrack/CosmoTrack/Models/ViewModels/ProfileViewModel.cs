using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }

        public ICollection<UserJournal> JournalEntries { get; set; }

        public int JournalEntriesCount { get; set; }

        public ICollection<Product> Products { get; set; }

        public int ProductCount { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public int ReviewsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

    }
}
