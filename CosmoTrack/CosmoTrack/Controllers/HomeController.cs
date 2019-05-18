using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmoTrack.Data;
using CosmoTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            

            var reviews = _context.Reviews.Where(r => r.MakePublic == true).ToList();

            foreach(var item in reviews)
            {
                item.UserProduct = _context.Products.FirstOrDefault(p => p.ID == item.ProductID);
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

            FollowsController followsController = new FollowsController(_context);

            await followsController.Create(follow);

            return RedirectToAction("Index");
        }
    }
}