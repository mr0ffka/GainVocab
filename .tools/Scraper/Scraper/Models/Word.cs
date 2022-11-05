using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }

        public Word(int id, string source, string translation)
        {
            Id = id;
            Source = source;
            Translation = translation;
        }
    }
}
