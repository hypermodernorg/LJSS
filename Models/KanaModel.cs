using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJSS.Models
{
    public class Kana
    {
        public int ID { get; set; }
        public string Pronunciation { get; set; }
        public string Hiragana { get; set; }
        public string Katakana { get; set; }
    }

    public class KanaQuizH
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Grade { get; set; }
        public string Level { get; set; }
        public string DateTime { get; set; }
    }

    public class KanaQuizK
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Grade { get; set; }
        public string Level { get; set; }
        public string DateTime { get; set; }
    }
}
