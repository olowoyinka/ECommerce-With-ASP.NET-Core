using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketWeb.Models.ViewModels;
using MarketWeb.Models;
using Microsoft.AspNetCore.Http;

namespace MarketWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _contextAccesor;

        public ProductsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccesor = contextAccessor;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (_contextAccesor.HttpContext.Session.GetString("Cart") == null)
            {
                List<Cart> IsCart = new List<Cart>
                {
                    new Cart(_context.Products.Find(1), 1)
                };

                _contextAccesor.HttpContext.Session.SetObjectAsJson("Cart", IsCart);

                IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>("Cart");

                IsCart.RemoveAt(0);

                _contextAccesor.HttpContext.Session.SetObjectAsJson("Cart", IsCart);
            }

            var applicationDbContext = _context.Products.Include(p => p.ApplicationUser).Include(p => p.Category).Include(p => p.Color).Include(p => p.Model).Include(p => p.Storage);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ApplicationUser)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Model)
                .Include(p => p.Storage)
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "Colors");
            ViewData["ModelID"] = new SelectList(_context.Models, "ModelID", "Models");
            ViewData["StorageID"] = new SelectList(_context.Storages, "StorageID", "Storages");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Image,SelfStartDate,SelfEndDate,Price,ApplicationUserId,CategoryID,ColorID,ModelID,StorageID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", product.ApplicationUserId);
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", product.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "Colors", product.ColorID);
            ViewData["ModelID"] = new SelectList(_context.Models, "ModelID", "Models", product.ModelID);
            ViewData["StorageID"] = new SelectList(_context.Storages, "StorageID", "Storages", product.StorageID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", product.ApplicationUserId);
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", product.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "Colors", product.ColorID);
            ViewData["ModelID"] = new SelectList(_context.Models, "ModelID", "Models", product.ModelID);
            ViewData["StorageID"] = new SelectList(_context.Storages, "StorageID", "Storages", product.StorageID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Image,SelfStartDate,SelfEndDate,Price,ApplicationUserId,CategoryID,ColorID,ModelID,StorageID")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", product.ApplicationUserId);
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", product.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Colors, "ColorID", "Colors", product.ColorID);
            ViewData["ModelID"] = new SelectList(_context.Models, "ModelID", "Models", product.ModelID);
            ViewData["StorageID"] = new SelectList(_context.Storages, "StorageID", "Storages", product.StorageID);
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
                .Include(p => p.ApplicationUser)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Model)
                .Include(p => p.Storage)
                .SingleOrDefaultAsync(m => m.ProductID == id);
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
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
