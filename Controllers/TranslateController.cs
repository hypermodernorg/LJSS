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
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Hosting;


// Important to remember for distribution is that the api key needs to be set in the environment variables 
// of host computer. 

namespace LJSS.Controllers
{
    public class TranslateController : Controller
    {
        public static IWebHostEnvironment _env;
        private readonly TranslateContext _context;

        public TranslateController(TranslateContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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


            // get translation
            TranslationClient client = TranslationClient.Create();
            TranslationResult result = client.TranslateText(tjapanese.TEnglish, LanguageCodes.Japanese);
            var resultText = result.TranslatedText;

            // get translated audio
            var audioClient = TextToSpeechClient.Create();
            var input = new SynthesisInput
            {
                Text = resultText
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
            var response = audioClient.SynthesizeSpeech(input, voiceSelection, audioConfig);

            // Write the response to the output file.

            string webRootPath = _env.WebRootPath;
            string translatesoundpath = webRootPath + "/assets/sounds/translations/" + resultText + ".mp3";
            string translatesoundhref = "http://localhost:5001" + "/assets/sounds/translations/" + resultText + ".mp3";
            if (System.IO.File.Exists(translatesoundpath) == false)
            {
                using var output = System.IO.File.Create(translatesoundpath);
                response.AudioContent.WriteTo(output);
            }

            return new JsonResult(resultText);

       
        }
    }
}