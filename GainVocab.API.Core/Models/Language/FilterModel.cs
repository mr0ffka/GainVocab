using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Language
{
    public class FilterModel
    {
        public string? Name { get; set; }
        public List<string>? Courses { get; set; }
    }
}
