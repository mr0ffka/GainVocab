using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Models
{
    public class ExampleSentence
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }

        public ExampleSentence(int id, int wordId, string source, string translation)
        {
            Id = id;
            WordId = wordId;
            Source = source;
            Translation = translation;
        }
    }
}
