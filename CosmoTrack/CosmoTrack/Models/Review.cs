using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    public class Review
    {
        public string UserID { get; set; }

        public int ProductID { get; set; }

        public string Review { get; set; }
    }
}
