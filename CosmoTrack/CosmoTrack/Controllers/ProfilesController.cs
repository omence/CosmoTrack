using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmoTrack.Data;
using CosmoTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;
using CosmoTrack.Models.ViewModels;

namespace CosmoTrack.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly CosmoTrackDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context2;

        private readonly IHostingEnvironment _he;

        public ProfilesController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext context2, IHostingEnvironment he)
        {
            _context = context;

            _userManager = userManager;

            _context2 = context2;

            _he = he;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            var UserID = _userManager.GetUserId(User);

            var user = _context2.Users.FirstOrDefault(u => u.Id == UserID);

            ProfileViewModel pvm = new ProfileViewModel();

            pvm.Profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserName == user.NickName);

            pvm.Products = await _context.Products.Where(u => u.UserID == UserID).Take(3).OrderByDescending(u => u.DateCreated).ToListAsync();

            pvm.ProductCount = _context.Products.Where(c => c.UserID == UserID).Count();

            pvm.Reviews = await _context.Reviews.Where(r => r.NickName == user.NickName).Take(3).OrderByDescending(r => r.DateCreated).ToListAsync();

            foreach(var p in pvm.Reviews)
            {
                p.UserProduct = await _context.Products.FirstOrDefaultAsync(r => r.ID == p.ProductID);
            }

            pvm.ReviewsCount = _context.Reviews.Where(r => r.NickName == user.NickName).Count();

            pvm.JournalEntries = await _context.UserJournals.Where(j => j.UserID == UserID).Take(3).OrderByDescending(j => j.DateCreated).ToListAsync();

            pvm.JournalEntriesCount = _context.UserJournals.Where(j => j.UserID == UserID).Count();

            pvm.FollowersCount = _context.Follows.Where(f => f.FollowingID == user.NickName).Count();

            pvm.FollowingCount = _context.Follows.Where(f => f.FollowerID == user.NickName).Count();

            return View(pvm);
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // GET: Profiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserName,ProfileImageURL,CurrentRegiment,ViewableByFollwers")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        [HttpGet]
        // GET: Profiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, [Bind("ID,UserName,ProfileImageURL,CurrentRegiment,ViewableByFollwers")] Profile profile)
        {
            if (id != profile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Profiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.ID == id);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile image)
        {

          if(image != null)
            {
                var fileName = Path.Combine(_he.WebRootPath, Path.GetFileName(image.FileName));

                image.CopyTo(new FileStream(fileName, FileMode.Create));

                var UserID = _userManager.GetUserId(User);

                var user = _context2.Users.FirstOrDefault(u => u.Id == UserID);

                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserName == user.NickName);

                profile.ProfileImageURL = "/" + Path.GetFileName(image.FileName);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public IActionResult Products()
        {
            return RedirectToAction("Index", "Products");
        }

        public IActionResult Reviews()
        {
            return RedirectToAction("Index", "Reviews");
        }

        public IActionResult Journal()
        {
            return RedirectToAction("Index", "UserJournals");
        }
    }
}
