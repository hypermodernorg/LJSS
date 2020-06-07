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
    public class WordModelsController : Controller
    {
        private readonly WordContext _context;

        public WordModelsController(WordContext context)
        {
            _context = context;
        }

        // GET: WordModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.WordModel.ToListAsync());
        }

        // GET: WordModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordModel = await _context.WordModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (wordModel == null)
            {
                return NotFound();
            }

            return View(wordModel);
        }

        // GET: WordModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WordModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,English,Japanese,Pronunciation,Definition,Notes,Category,Synonyms,Example")] WordModel wordModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wordModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wordModel);
        }

        // GET: WordModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordModel = await _context.WordModel.FindAsync(id);
            if (wordModel == null)
            {
                return NotFound();
            }
            return View(wordModel);
        }

        // POST: WordModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,English,Japanese,Pronunciation,Definition,Notes,Category,Synonyms,Example")] WordModel wordModel)
        {
            if (id != wordModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wordModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordModelExists(wordModel.ID))
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
            return View(wordModel);
        }

        // GET: WordModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordModel = await _context.WordModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (wordModel == null)
            {
                return NotFound();
            }

            return View(wordModel);
        }

        // POST: WordModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wordModel = await _context.WordModel.FindAsync(id);
            _context.WordModel.Remove(wordModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordModelExists(int id)
        {
            return _context.WordModel.Any(e => e.ID == id);
        }
    }
}
