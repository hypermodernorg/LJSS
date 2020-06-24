namespace LJSS.Models
{
    public class TransModel
    {
        public string TJapanese { get; set; }
        public string TEnglish { get; set; }
    }
    public class WordModelTrans
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

    public class KanaTrans
    {
        public int ID { get; set; }
        public string Pronunciation { get; set; }
        public string Hiragana { get; set; }
        public string Katakana { get; set; }
    
    }
}
