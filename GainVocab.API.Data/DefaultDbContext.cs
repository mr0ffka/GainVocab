using GainVocab.API.Data.Configurations;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data
{
    public class DefaultDbContext : IdentityDbContext<APIUser>
    {
        public DefaultDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new APIUserCourseConfiguration());
            modelBuilder.ApplyConfiguration(new CourseDataConfiguration());
            modelBuilder.ApplyConfiguration(new CourseDataExampleConfiguration());
            modelBuilder.ApplyConfiguration(new SupportIssueConfiguration());
            modelBuilder.ApplyConfiguration(new SupportIssueTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseProgressConfiguration());
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseData> CourseData { get; set; }
        public DbSet<APIUserCourse> APIUserCourse { get; set; }
        public DbSet<CourseDataExample> CourseDataExample { get; set; }
        public DbSet<SupportIssue> SupportIssue { get; set; }
        public DbSet<SupportIssueType> SupportIssueType { get; set; }
        public DbSet<CourseProgress> CourseProgress { get; set; }
    }
}
