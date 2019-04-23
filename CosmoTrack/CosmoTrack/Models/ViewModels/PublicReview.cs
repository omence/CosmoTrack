using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models.ViewModels
{
    public class PublicReview
    {
        public string UserName { get; set; }

        public string UserReview { get; set; }

        public Product product { get; set; }
    }
}
