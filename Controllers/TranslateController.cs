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
using Google.Cloud.TextToSpeech;
using Google.Cloud.TextToSpeech.V1;
using System.Net.Http;
using System.Net.Http.Headers;


// Important to remember for distribution is that the api key needs to be set in the environment variables 
// of host computer. 

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

        public string GetKanaK(List<KanaTrans> kanas, string dbjword)
        {
            string transliteration = "";
            string syl = "";
            int len = dbjword.Length;


            for (int i = 0; i < len; i++)
            {
                syl = dbjword[i].ToString();

                foreach (var kana in kanas)
                {
                    if (kana.Katakana == syl)
                    {
                        transliteration += kana.Pronunciation;
                    }

                }
            }
            return transliteration;
        }
        public string GetKanaH(List<KanaTrans> kanas, string dbjword)
        {
            string transliteration = "";
            string syl = "";
            int len = dbjword.Length;


            for (int i = 0; i < len; i++)
            {
                syl = dbjword[i].ToString();

                foreach (var kana in kanas)
                {
                    if (kana.Hiragana == syl)
                    {
                        transliteration += kana.Pronunciation;
                    }

                }
            }
            return transliteration;
        }

        public ActionResult UString()
        {
            var tjapanese = new TransModel()
            {
                // Gets the input
                TEnglish = Request.Form["estring"]
            };

            // process translation request
            // 1. process english into list of words.
            string[] uwords = tjapanese.TEnglish.Split(' ');

            // 2. get corresponding japanese words from database.
            var dbwords = _context.WordModelTrans
                .FromSqlRaw("SELECT * FROM WordModel")
                .ToList();

            var kanas = _context.KanaTrans
                .FromSqlRaw("SELECT * FROM Kana")
                .ToList();

            string buildJapaneseOutput = "";
            string buildTransliterationOutput = "";

            foreach (var uword in uwords)
            {
                foreach (var dbword in dbwords)
                {
                    var english = dbword.English;
                    var japanese = dbword.Japanese;
                    var transliteration = dbword.Pronunciation;

                    if (english == uword)
                    {
                        if (dbword.System == "Hiragana")
                        {
                            buildJapaneseOutput += japanese + " | ";
                            buildTransliterationOutput += GetKanaH(kanas, dbword.Japanese) + " | ";
                            break;
                        }
                        else if (dbword.System == "Katakana")
                        {
                            buildJapaneseOutput += japanese + " | ";
                            buildTransliterationOutput += GetKanaK(kanas, dbword.Japanese) + " | ";
                            break;
                        }

                        else if (dbword.System == "Kanji")
                        {
                            buildJapaneseOutput += japanese + " | ";
                            buildTransliterationOutput += transliteration + " | ";
                            break;
                        }
                    }
                }
            }


            // end process translation request

            //////////////////////////////////////////////
            // try texttospeech
            var client = TextToSpeechClient.Create();
            var input = new SynthesisInput
            {
                Text = "こんにちは"
            };

            // Build the voice request.
            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = "ja-JP",
                SsmlGender = SsmlVoiceGender.Female,
                Name= "ja-JP-Wavenet-A"
            };

            // Specify the type of audio file.
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            // Perform the text-to-speech request.
            var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            // Write the response to the output file.
            //using (var output = System.IO.File.Create("output.mp3"))
            //{
            //    response.AudioContent.WriteTo(output);
            //}
            // end try text to speech
            //////////////////////////////////////////////
         
            return new JsonResult(buildJapaneseOutput.Concat("\n" + buildTransliterationOutput));
        }
    }
}