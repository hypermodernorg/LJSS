using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LJSS.Data;
using LJSS.Models;

namespace LJSS.Controllers
{
    public class KanaController : Controller
    {
        private readonly KanaContext _context;

        public KanaController(KanaContext context)
        {
            _context = context;
        }

        // GET: Kanas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kana.ToListAsync());
        }

        // GET: Kanas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kana = await _context.Kana
                .FirstOrDefaultAsync(m => m.ID == id);
            if (kana == null)
            {
                return NotFound();
            }

            return View(kana);
        }

        // GET: Kanas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kanas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Pronunciation,Hiragana,Katakana")] Kana kana)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kana);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kana);
        }

        // GET: Kanas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kana = await _context.Kana.FindAsync(id);
            if (kana == null)
            {
                return NotFound();
            }
            return View(kana);
        }

        // POST: Kanas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Pronunciation,Hiragana,Katakana")] Kana kana)
        {
            if (id != kana.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kana);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KanaExists(kana.ID))
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
            return View(kana);
        }

        // GET: Kanas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kana = await _context.Kana
                .FirstOrDefaultAsync(m => m.ID == id);
            if (kana == null)
            {
                return NotFound();
            }

            return View(kana);
        }

        // POST: Kanas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kana = await _context.Kana.FindAsync(id);
            _context.Kana.Remove(kana);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KanaExists(int id)
        {
            return _context.Kana.Any(e => e.ID == id);
        }
    }
}
