using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketWeb.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using MarketWeb.Models;

namespace MarketWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _contextAccesor;

        public OrdersController(ApplicationDbContext context, IHttpContextAccessor contextAccesor)
        {
            _context = context;
            _contextAccesor = contextAccesor;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderName,OrderDate,PaymentType,Status,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderName,OrderDate,PaymentType,Status,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        public void ExportContentToCSV()
        {
            StringWriter strw = new StringWriter();

            strw.WriteLine("\"OrderID\",\"OrderName\",\"OrderDate\",\"PaymentType\",\"Status\",\"CustomerName\",\"CustomerPhone\",\"CustomerEmail\",\"CustomerAddress\"");

            _contextAccesor.HttpContext.Response.Clear();
            _contextAccesor.HttpContext.Response.Headers.Add("content-disposition",
                                                                string.Format("attachment;filename=NewsListing_{0}.csv", DateTime.Now));

            _contextAccesor.HttpContext.Response.ContentType = "text/csv";

            var listOrder = _context.Orders.OrderBy(x => x.OrderID).ToList();
            foreach (var order in listOrder)
            {
                strw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\"",
                                                order.OrderID, order.OrderName, order.OrderDate, order.PaymentType, order.Status, order.CustomerName, order.CustomerPhone, order.CustomerEmail, order.CustomerAddress));
            }

            _contextAccesor.HttpContext.Response.WriteAsync(strw.ToString()).Wait();
        }
    }
}
