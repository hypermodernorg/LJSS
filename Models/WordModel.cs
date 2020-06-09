using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace LJSS.Models
{
    public class WordModel
    {
        public int ID { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Pronunciation { get; set; }
        public string Definition { get; set; }
        public string Notes { get; set; }
        public string Category { get; set; }
        public string Synonyms { get; set; }
        public string Example { get; set; }
        public string System { get; set; }
    }
}
