using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmoTrack.Data;
using CosmoTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CosmoTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly CosmoTrackDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;

            _userManager = userManager;
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
    }
}