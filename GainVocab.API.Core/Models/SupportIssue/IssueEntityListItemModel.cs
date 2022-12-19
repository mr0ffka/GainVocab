using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.SupportIssue
{
    public class IssueEntityListItemModel
    {
        public string? EntityId { get; set; }
        public string? CourseName { get; set; }
        public string? LanguageFrom { get; set; }
        public string? LanguageTo { get; set; }
        public string? Source { get; set; }
        public string? Translation { get; set; }
    }
}
