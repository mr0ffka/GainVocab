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
    public class CourseDataExampleConfiguration : IEntityTypeConfiguration<CourseDataExample>
    {
        public void Configure(EntityTypeBuilder<CourseDataExample> builder)
        {
            builder.ToTable("CourseDataExample");

            builder.HasIndex(x => x.PublicId, "IX_CourseDataExample_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();

            builder.HasOne(e => e.CourseData)
                   .WithMany(c => c.Examples)
                   .HasForeignKey(c => c.CourseDataId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_CourseDataExample_CourseDataId");

            builder.Navigation(b => b.CourseData)
                   .AutoInclude();
        }
    }
}
