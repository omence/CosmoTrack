using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    public class Profile
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string ProfileImageURL { get; set; }

        public string CurrentRegiment { get; set; }

        public bool ViewableByFollwers { get; set; }

    }
}
