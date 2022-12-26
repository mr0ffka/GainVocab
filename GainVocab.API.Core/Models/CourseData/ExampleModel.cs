using FluentValidation;
using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.CourseData
{
    public class ExampleModel
    {
        public string Source { get; set; }
        public string Translation { get; set; }
    }
}
