using CsvHelper.Configuration;
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
        public int Id { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }
    }

    public sealed class ImportDataModelMap : ClassMap<ImportDataModel>
    {
        public ImportDataModelMap()
        {
            Map(m => m.Id);
            Map(m => m.Source);
            Map(m => m.Translation);
        }
    }
}
