using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.CourseData
{
    public class ImportDataExampleModel
    {
        [Index(0)]
        public string Id { get; set; }
        [Index(1)]
        public string WordId { get; set; }
        [Index(2)]
        public string Source { get; set; }
        [Index(3)]
        public string Translation { get; set; }
    }
}
