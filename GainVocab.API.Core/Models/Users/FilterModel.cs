using GainVocab.API.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Users
{
    public class FilterModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<UserRoles>? Roles { get; set; }
    }
}
