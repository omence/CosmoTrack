using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    public class Review
    {
        public int ID { get; set; }

        public string ProfileImageURL { get; set; }

        public string NickName { get; set; }

        public bool MakePublic { get; set; }

        public string UserID { get; set; }

        public int ProductID { get; set; }

        public int Rating { get; set; }

        public string UserReview { get; set; }

        public string VideoReviewURL { get; set; }


        public DateTime DateCreated { get; set; }

        public string Tags { get; set; }

        //Nav prop
        public Product UserProduct { get; set; }
    }
}
