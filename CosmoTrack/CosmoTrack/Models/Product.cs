using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string ImageURL { get; set; }

        public string Brand { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Ingredients { get; set; }

        public string Description { get; set; }

        public bool HasReview { get; set; }

        //Nav props
        public ICollection<ProductJournal> ProductJournals { get; set; }

        public Review Reviews { get; set; }


    }
}
