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

            builder.HasIndex(x => x.PublicId, "IX_APIUserCourse_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();

            builder
                .HasOne(uc => uc.APIUser)
                .WithMany(u => u.Courses)
                .HasForeignKey(uc => uc.APIUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(uc => uc.Course)
                .WithMany(c => c.Users)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(uc => uc.CourseProgress)
                .WithOne(cp => cp.UserCourse)
                .HasForeignKey<CourseProgress>(cp => cp.UserCourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
