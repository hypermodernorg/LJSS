using System;
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
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LJSS.Controllers
{
    public class VocabularyController : Controller
    {
        private readonly VocabularyContext _context;

        public static IWebHostEnvironment _env;
        public VocabularyController(VocabularyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: WordModels
        public async Task<IActionResult> Index()
        {

            string webRootPath = _env.WebRootPath;
            string vocabularysound = webRootPath + "/assets/sounds/vocabulary/";
            string vocabularySoundHref = "http://localhost:5001/assets/sounds/vocabulary/";

            ViewData["vocabularySoundHref"] = vocabularySoundHref;
            ViewData["vocabularySound"] = vocabularysound;
            ViewData["mp3"] = ".mp3";
            ViewData["isauthorized"] = User.IsInRole("Admin");

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
        public async Task<IActionResult> Create([Bind("ID,English,Japanese,Pronunciation,Definition,Notes,Category,Synonyms,Example,System,UserName")] WordModel wordModel)
        {
            if (ModelState.IsValid)
            {
                wordModel.UserName = User.Identity.Name;
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,English,Japanese,Pronunciation,Definition,Notes,Category,Synonyms,Example,System,UserName")] WordModel wordModel)
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

        public ActionResult VocabularySounds()
        {
            string webRootPath = _env.WebRootPath;
            string vocabularyText = Request.Form["vocabularystring"];
            string vocabularySound = webRootPath + "/assets/sounds/vocabulary/" + vocabularyText + ".mp3";
            string vocabularySoundHref = "http://localhost:5001" + "/assets/sounds/vocabulary/" + vocabularyText + ".mp3";

            //////////////////////////////////////////////
            // try texttospeech
            var client = TextToSpeechClient.Create();
            var input = new SynthesisInput
            {
                Text = vocabularyText
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

            if (System.IO.File.Exists(vocabularySound) == false)
            {
                using var output = System.IO.File.Create(vocabularySound);
                response.AudioContent.WriteTo(output);
            }


            // end try text to speech
            //////////////////////////////////////////////

            return new JsonResult(vocabularySoundHref);
        }

    }
}
