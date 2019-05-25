using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CosmoTrack.Models
{
    public class Profile
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Upload File")]
        public string ProfileImageURL { get; set; }

        [Display(Name = "Current Regiment")]
        public string CurrentRegiment { get; set; }

        public bool ViewableByFollwers { get; set; }

   
    }
}
