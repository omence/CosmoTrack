using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models
{
    
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string NickName { get; set; }


       
    }

  


}

