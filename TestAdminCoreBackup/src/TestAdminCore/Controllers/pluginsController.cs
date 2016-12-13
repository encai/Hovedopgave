using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestAdminCore.Data;
using TestAdminCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace TestAdminCore.Controllers
{
    [Authorize]
    public class pluginsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public pluginsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: plugins
        public async Task<IActionResult> Index()
        {
            return View(await _context.plugin.ToListAsync());
        }

        // GET: plugins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.plugin.SingleOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }

            return View(plugin);
        }

        [Authorize(Roles = "Administrator")]
        // GET: plugins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: plugins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url")] plugin plugin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plugin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(plugin);
        }

        // GET: plugins/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.plugin.SingleOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }
            return View(plugin);
        }

        // POST: plugins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url")] plugin plugin)
        {
            if (id != plugin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plugin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!pluginExists(plugin.Id))
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
            return View(plugin);
        }

        // GET: plugins/Delete/5'
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.plugin.SingleOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }

            return View(plugin);
        }

        // POST: plugins/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plugin = await _context.plugin.SingleOrDefaultAsync(m => m.Id == id);
            _context.plugin.Remove(plugin);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool pluginExists(int id)
        {
            return _context.plugin.Any(e => e.Id == id);
        }
    }
}
