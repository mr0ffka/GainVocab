using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GainVocab.API.Data.Configurations
{ 
    public class APIUserCourseConfiguration : IEntityTypeConfiguration<APIUserCourse>
    {
        public void Configure(EntityTypeBuilder<APIUserCourse> builder)
        {
            builder.ToTable("APIUserCourse");

            builder
                .HasKey(uc => new { uc.APIUserId, uc.CourseId });

            builder
                .HasOne(uc => uc.APIUser)
                .WithMany(uc => uc.Courses)
                .HasForeignKey(uc => uc.APIUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(uc => uc.Course)
                .WithMany(uc => uc.Users)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
