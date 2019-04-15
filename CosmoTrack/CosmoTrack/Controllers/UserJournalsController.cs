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
    public class UserJournalsController : Controller
    {
        private readonly CosmoTrackDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserJournalsController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;

            _userManager = userManager;
        }

        // GET: UserJournals
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserJournals.ToListAsync());
        }

        // GET: UserJournals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userJournal = await _context.UserJournals
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userJournal == null)
            {
                return NotFound();
            }

            return View(userJournal);
        }

        // GET: UserJournals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserJournals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,JournalEntry")] UserJournal userJournal)
        {
            userJournal.UserID = _userManager.GetUserId(User);

            userJournal.DateCreated = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(userJournal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userJournal);
        }

        // GET: UserJournals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userJournal = await _context.UserJournals.FindAsync(id);
            if (userJournal == null)
            {
                return NotFound();
            }
            return View(userJournal);
        }

        // POST: UserJournals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,JournalEntry,DateCreated")] UserJournal userJournal)
        {
            if (id != userJournal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userJournal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserJournalExists(userJournal.ID))
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
            return View(userJournal);
        }

        // GET: UserJournals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userJournal = await _context.UserJournals
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userJournal == null)
            {
                return NotFound();
            }

            return View(userJournal);
        }

        // POST: UserJournals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userJournal = await _context.UserJournals.FindAsync(id);
            _context.UserJournals.Remove(userJournal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserJournalExists(int id)
        {
            return _context.UserJournals.Any(e => e.ID == id);
        }
    }
}
