using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LJSS.Data;
using LJSS.Models;
using Google.Cloud.TextToSpeech.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LJSS.Controllers
{
    public class KanaController : Controller
    {
        private readonly KanaContext _context;
        public static IWebHostEnvironment _env;

        public KanaController(KanaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Kanas
        public async Task<IActionResult> Index()
        {

            string webRootPath = _env.WebRootPath;
            string kanasound = webRootPath + "/assets/sounds/kana/";
            string kanasoundhreg = "http://localhost:5001/assets/sounds/kana/";

            ViewData["kanasoundhreg"] = kanasoundhreg;
            ViewData["kanasound"] = kanasound;
            ViewData["mp3"] = ".mp3";
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

        public ActionResult KanaSounds()
        {
            string webRootPath = _env.WebRootPath;
            string kanaText = Request.Form["kanastring"];
            string kanasound = webRootPath + "/assets/sounds/kana/" + kanaText + ".mp3";
            string kanasoundhreg = "http://localhost:5001" + "/assets/sounds/kana/" + kanaText + ".mp3";

            //////////////////////////////////////////////
            // try texttospeech
            var client = TextToSpeechClient.Create();
            var input = new SynthesisInput
            {
                Text = kanaText
            };

            // Build the voice request.
            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = "ja-JP",
                SsmlGender = SsmlVoiceGender.Female,
                Name = "ja-JP-Wavenet-A"
            };

            // Specify the type of audio file.
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            // Perform the text-to-speech request.
            var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            // Write the response to the output file.
            
            if (System.IO.File.Exists(kanasound) == false)
            {
                using var output = System.IO.File.Create(kanasound);
                response.AudioContent.WriteTo(output);
            }

           
            // end try text to speech
            //////////////////////////////////////////////

            return new JsonResult(kanasoundhreg);
        }
    }
}
