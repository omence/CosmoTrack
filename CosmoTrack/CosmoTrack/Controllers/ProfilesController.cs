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

namespace CosmoTrack.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly CosmoTrackDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _application;

        public ProfilesController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext application)
        {
            _context = context;
            _userManager = userManager;
            _application = application;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            var userID = _userManager.GetUserId(User);
            var user = _application.Users.FirstOrDefault(u => u.Id == userID);
            return View(_context.Profiles.FirstOrDefault(p => p.UserName == user.NickName));
        }

       // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profile profile)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,ProfileImageURL,CurrentRegiment")] Profile profile)
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

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.ID == id);
        }
    }
}
