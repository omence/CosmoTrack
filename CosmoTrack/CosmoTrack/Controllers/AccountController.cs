using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CosmoTrack.Models;
using CosmoTrack.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CosmoTrack.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
          
        }
        /// <summary>
        /// Sends home Page to Browser
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Gets registration info from user and creates a user in DB
        /// </summary>
        /// <param name="rvm"></param>
        /// <returns>View back to home</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {   //setting values to input from user
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = rvm.Email,
                    Email = rvm.Email,
                    FirstName = rvm.FirstName,
                    LastName = rvm.LastName,
                };

                //creates passsword if password is in valid format
                var result = await _userManager.CreateAsync(user, rvm.Password);

                //creat a number of different claims
                if (result.Succeeded)
                {
                    Claim fullNameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");

                    //list to hold the claims
                    List<Claim> claims = new List<Claim> { fullNameClaim};

                    //returns list of claims to user manager
                    await _userManager.AddClaimsAsync(user, claims);

                    //sends user to home page after sign in
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(rvm);
        }

        /// <summary>
        /// Sends login View to Browser
        /// </summary>
        /// <returns>Login View</returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Logs user in
        /// </summary>
        /// <param name="lvm"></param>
        /// <returns>Home View</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(lvm.Email);
                   
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.TryAddModelError(string.Empty, "Invalid Login Attempt");

            return View(lvm);
        }

        /// <summary>
        /// Logs user out
        /// </summary>
        /// <returns>Home View</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}