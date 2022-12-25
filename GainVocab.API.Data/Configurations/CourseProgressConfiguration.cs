using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Configurations
{
    public class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
    {
        public void Configure(EntityTypeBuilder<CourseProgress> builder)
        {
            builder.ToTable("CourseProgress");

            builder.HasIndex(x => x.PublicId, "IX_CourseProgress_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();

            builder.HasOne(e => e.CurrentCourseData)
               .WithMany(e => e.Progresses)
               .HasForeignKey(e => e.CurrentCourseDataId)
               .OnDelete(DeleteBehavior.SetNull)
               .HasConstraintName("FK_CourseProgress_CurrentCourseDataId");

        }
    }
}