using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GainVocab.API.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasIndex(x => x.PublicId, "IX_Courses_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();

            builder.HasOne(l => l.LanguageFrom)
                   .WithMany(c => c.CoursesFrom)
                   .HasForeignKey(c => c.LanguageFromId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Course_LanguageFromId");

            builder.Navigation(b => b.LanguageFrom)
                   .AutoInclude();

            builder.HasOne(l => l.LanguageTo)
                   .WithMany(c => c.CoursesTo)
                   .HasForeignKey(c => c.LanguageToId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Course_LanguageToId");

            builder.Navigation(b => b.LanguageTo)
                   .AutoInclude();

            builder.HasMany(e => e.Data)
                   .WithOne(e => e.Course)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Users)
                   .WithOne(e => e.Course)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
