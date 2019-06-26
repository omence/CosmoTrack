using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    public class UserJournal
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string Topic { get; set; }

        public string JournalEntry { get; set; }

        public bool ViewableByFollwers { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
