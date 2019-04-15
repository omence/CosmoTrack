using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    public class ProductJournal
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public string JournalEntry { get; set; }
    }
}
