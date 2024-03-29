﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Models
{
    public class Language
    {
        public Language()
        {
            CoursesFrom = new HashSet<Course>();
            CoursesTo = new HashSet<Course>();
        }

        [Key]
        public long Id { get; set; }
        public string PublicId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Course> CoursesFrom { get; set; }
        public virtual ICollection<Course> CoursesTo { get; set; }
    }
}
