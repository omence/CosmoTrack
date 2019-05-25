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

            return View(await _context.Profiles.FirstOrDefaultAsync(p => p.UserName == user.NickName));
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,ProfileImageURL,CurrentRegiment,ViewableByFollwers")] Profile profile)
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
            return View(profile);
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
    }
}
