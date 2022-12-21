using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.CourseData
{
    public class ImportDataModel
    {
        [Index(0)]
        public string Id { get; set; }
        [Index(1)]
        public string Source { get; set; }
        [Index(2)]
        public string Translation { get; set; }
    }
}
