using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CosmoTrack.Data;
using CosmoTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CosmoTrack.Controllers
{
    [Authorize]
    public class FollowsController : Controller
    {
        private readonly CosmoTrackDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context2;

        public FollowsController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext context2)
        {
            _context = context;

            _userManager = userManager;

            _context2 = context2;
        }

        // GET: Follows
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var user = _context2.Users.FirstOrDefault(u => u.Id == userId);

            return View(await _context.Follows.Where(f => f.FollowerID == user.NickName).ToListAsync());
        }

        // GET: Follows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follows
                .FirstOrDefaultAsync(m => m.ID == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // POST: Follows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Follow follow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(follow);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Follows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follows.FindAsync(id);
            if (follow == null)
            {
                return NotFound();
            }
            return View(follow);
        }

        // POST: Follows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FollowerID,FollowingID")] Follow follow)
        {
            if (id != follow.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(follow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowExists(follow.ID))
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
            return View(follow);
        }

        // GET: Follows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follows
                .FirstOrDefaultAsync(m => m.ID == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var follow = await _context.Follows.FindAsync(id);
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowExists(int id)
        {
            return _context.Follows.Any(e => e.ID == id);
        }
    }
}
