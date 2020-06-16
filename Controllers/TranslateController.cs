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
    public class TranslateController : Controller
    {
        private readonly TranslateContext _context;

        public TranslateController(TranslateContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> UString()
        public ActionResult UString()
        {


            var tjapanese = new TransModel()
            {
                // Gets the input
                TEnglish = Request.Form["estring"]

            };





            // process translation request
            // 1. process english into list of words.
            string[] ewords = tjapanese.TEnglish.Split(' ');

            // 2. get corresponding japanese words from database.
            var words = _context.WordModelTrans
                .FromSqlRaw("SELECT * FROM WordModel")
                .ToList();

            string buildJapaneseOutput = "";
            string buildTransliterationOutput = "";
            foreach (var eword in ewords)
            {
                foreach (var word in words)
                {
                    var english = word.English;
                    var japanese = word.Japanese;
                    var transliteration = word.Pronunciation;

                    if (english == eword)
                    {
                        if (word.System == "Hiragana")
                        {
                            buildJapaneseOutput += japanese + " | ";
                            buildTransliterationOutput += transliteration + " | ";

                        }    
                    }
                }
            }


            // end process translation request

            //return new JsonResult(buildJapaneseOutput);

            // How do i return both json results?
            return new JsonResult(buildJapaneseOutput.Concat("\n" + buildTransliterationOutput));
        }
    }
}