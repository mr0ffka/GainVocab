using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class Course
    {
        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public string Name { get; set; }
        public long LanguageFromId { get; set; }
        public long LanguageToId { get; set; }

        public virtual Language? LanguageFrom { get; set; }
        public virtual Language? LanguageTo { get; set; }
        public ICollection<APIUserCourse> Users { get; set; }
    }
}
