using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GainVocab.API.Core.Extensions;

namespace GainVocab.API.Core.Models.Language
{
    public class ListItemModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Courses { get; set; }
    }
}
