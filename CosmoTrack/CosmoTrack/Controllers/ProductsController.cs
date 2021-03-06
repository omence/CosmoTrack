﻿using System;
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
    public class ProductsController : Controller
    {
        private readonly CosmoTrackDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context2;

        public ProductsController(CosmoTrackDbContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext context2)
        {
            _context = context;
            _context2 = context2;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(string SearchString)
        {
            var userId = _userManager.GetUserId(User);

            var products = await _context.Products.Where(p => p.UserID == userId).ToListAsync();

            foreach(var p in products)
            {
                p.Reviews = await _context.Reviews.FirstOrDefaultAsync(r => r.ProductID == p.ID);
            }

            if (!String.IsNullOrEmpty(SearchString))
            { 

                var products2 = products.Where(p => p.Tags.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                var splitSearch = SearchString.Split(" ");

                foreach (var i in splitSearch)
                {
                    var temp = products.Where(r => r.Tags.IndexOf(i.ToString(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();

                    products2 = products2.Union(temp).ToList();

                }

                return View(products2);
            }
            return View(products);


        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ID == id);
            product.Reviews = await _context.Reviews.FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ImageURL,Brand,Name,Price,Ingredients,Description")] Product product)
        {
            product.Tags = $"{product.Brand} {product.Name} {product.Brand} {product.Ingredients}";

            product.UserID = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,ImageURL,Brand,Name,Price,Ingredients,Description")] Product product)
        {
            product.Tags = $"{product.Brand} {product.Name} {product.Brand} {product.Ingredients}";

            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.UserID = _userManager.GetUserId(User);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }

        public async Task<IActionResult> Review(int id, bool MakePublic, int Rating, string UserReview, string VideoReviewURL, string ImageOneURL, string ImageTwoURL, string ImageThreeURL, string ImageFourURL)
        {
            var userId = _userManager.GetUserId(User);

            var user = _context2.Users.FirstOrDefault(u => u.Id == userId);

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);

            Review review = new Review();
            review.DateCreated = DateTime.Now;
            review.ProductID = id;
            review.NickName = user.NickName;
            review.MakePublic = MakePublic;
            review.Rating = Rating;
            review.UserReview = UserReview;
            review.VideoReviewURL = VideoReviewURL;
            review.Tags = product.Tags;

            

            product.HasReview = true;

            await _context.SaveChangesAsync();

            ReviewsController reviewsController = new ReviewsController(_context);

            await reviewsController.Create(review);

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
