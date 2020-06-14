using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LJSS.Models;

namespace LJSS.Controllers
{
    public class TranslateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UString()
        {
            //string[] keys = Request.Form.Keys.ToArray();
            // string keyValue = keys.englishText;

            var tjapanese = new TransModel()
            {
                //Gets the input
                TEnglish = Request.Form["estring"]

            };


            // process translation request
            // 1. process english into list of words.
            string[] ewords = tjapanese.TEnglish.Split(' ');

            // 2. get corresponding japanese words from database.


            // 3. process japanese word order
            // 4. output japanese word or sentance to html.
            // end process translation request

            return new JsonResult(tjapanese.TEnglish);
        }
    }
}