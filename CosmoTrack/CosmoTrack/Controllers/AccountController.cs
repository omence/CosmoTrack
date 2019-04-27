using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;
using CosmoTrack.Data;
using CosmoTrack.Models;
using CosmoTrack.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CosmoTrack.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
          
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
            var email = _context.Users.FirstOrDefault(e => e.Email == rvm.Email);

            var nickName = _context.Users.FirstOrDefault(n => n.NickName == rvm.NickName);

            if (email == null)
            {
                if (nickName == null)
                {

                    if (ModelState.IsValid)
                    {   //setting values to input from user
                        ApplicationUser user = new ApplicationUser()
                        {
                            UserName = rvm.Email,
                            Email = rvm.Email,
                            NickName = rvm.NickName
                        };

                        //creates passsword if password is in valid format
                        var result = await _userManager.CreateAsync(user, rvm.Password);

                        //creat a number of different claims
                        if (result.Succeeded)
                        {
                            Claim NickNameClaim = new Claim("NickName", $"{user.NickName}");

                            //list to hold the claims
                            List<Claim> claims = new List<Claim> { NickNameClaim };

                            //returns list of claims to user manager
                            await _userManager.AddClaimsAsync(user, claims);

                            //sends user to home page after sign in
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            return RedirectToAction("Index", "Home");
                        }
                    }

                    return View(rvm);
                }

                ModelState.AddModelError("NickName", "This user name already exisits, please choose another");

                return View(rvm);
            }
            ModelState.AddModelError("Email", "An account with associated with this email address already exists, please login or create your account with another email");

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


        /// </summary>
        /// <param name="lvm">Login view model</param>
        /// <returns>Signed in home index view</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

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