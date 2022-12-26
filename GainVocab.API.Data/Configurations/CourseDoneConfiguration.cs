using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GainVocab.API.Data.Configurations
{
    public class CourseDoneConfiguration : IEntityTypeConfiguration<CourseDone>
    {
        public void Configure(EntityTypeBuilder<CourseDone> builder)
        {
            builder.ToTable("CoursesDone");

            builder
                .HasKey(uc => new { uc.APIUserId, uc.CourseId });

            builder
                .HasOne(uc => uc.APIUser)
                .WithMany(u => u.CoursesDone)
                .HasForeignKey(pd => pd.APIUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UsersDone)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
