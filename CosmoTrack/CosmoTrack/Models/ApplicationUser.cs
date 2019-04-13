using Microsoft.AspNetCore.Identity;
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
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "User Name")]
        public string NickName { get; set; }

        public string ProfileImageURL { get; set; }

        public ICollection<Follow> Followers { get; set; }

        public ICollection<Follow> Following { get; set; }
        
    }

  


}

