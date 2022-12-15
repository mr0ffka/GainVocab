using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.CourseData
{
    public class ListItemModel
    {
        public string PublicId { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }
    }
}
