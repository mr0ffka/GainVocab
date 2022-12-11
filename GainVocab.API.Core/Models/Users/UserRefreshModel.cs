using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Users
{
    public class UserRefreshModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
