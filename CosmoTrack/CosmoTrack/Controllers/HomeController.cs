using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmoTrack.Data;
using CosmoTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CosmoTrack.Controllers;

namespace CosmoTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly CosmoTrackDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context2;

        public HomeController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext context2)
        {
            _context = context;

            _userManager = userManager;

            _context2 = context2;
        }
        public async Task<IActionResult> Index(string SearchString)
        {
            var reviews = _context.Reviews.Where(r => r.MakePublic == true).OrderByDescending(x => x.DateCreated).ToList();

            foreach(var item in reviews)
            {
                item.UserProduct = _context.Products.FirstOrDefault(p => p.ID == item.ProductID);
            }
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                var reviews1 = reviews.Where(r => r.NickName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                var reviews2 = reviews.Where(r => r.UserProduct.Tags.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                reviews1 = reviews1.Concat(reviews2).ToList();

                var splitSearch = SearchString.Split(" ");

                foreach (var i in splitSearch)
                {
                    var temp = reviews.Where(r => r.UserProduct.Tags.IndexOf(i.ToString(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                    reviews1 = reviews1.Union(temp).ToList();

                }

                return View(reviews1);
            }

            return View(reviews);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string nickName)
        {
            var thisUserID = _userManager.GetUserId(User);

            var thisUserNickName = _context2.Users.FirstOrDefault(u => u.Id == thisUserID);

            Follow follow = new Follow();

            follow.FollowingID = nickName;

            follow.FollowerID = thisUserNickName.NickName;

            FollowsController followsController = new FollowsController(_context, _userManager, _context2);

            await followsController.Create(follow);

            return RedirectToAction("Index");
        }
    }
}