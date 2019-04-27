using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CosmoTrack.Models.ViewModels
{
    public class RegisterViewModel
    { 

        [Required(ErrorMessage = "User Name is Required")]
        [Display(Name = "User Name")]
        public string NickName { get; set; }

        
        [Required(ErrorMessage = "Email Address is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Confirmation is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Does Not Match")]
        public string ConfirmPassword { get; set; }
    }
}
