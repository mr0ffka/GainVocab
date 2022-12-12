using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Course
{
    public class FilterModel
    {
        public string? Name { get; set; }
        public string? LanguageFrom { get; set; }
        public string? LanguageTo { get; set; }
    }
}
