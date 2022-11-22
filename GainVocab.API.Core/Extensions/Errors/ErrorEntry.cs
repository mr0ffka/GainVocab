using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Extensions.Errors
{
    public class ErrorEntry
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
    }
}
