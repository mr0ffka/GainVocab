using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GainVocab.API.Data.Configurations
{ 
    public class CourseProgressDataDoneConfiguration : IEntityTypeConfiguration<CourseProgressDataDone>
    {
        public void Configure(EntityTypeBuilder<CourseProgressDataDone> builder)
        {
            builder.ToTable("CourseProgressDataDone");

            builder
                .HasKey(pd => new { pd.CourseProgressId, pd.CourseDataId });

            builder
                .HasOne(pd => pd.CourseProgress)
                .WithMany(p => p.DataDone)
                .HasForeignKey(pd => pd.CourseProgressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(pd => pd.CourseData)
                .WithMany(d => d.ProgressesDone)
                .HasForeignKey(pd => pd.CourseDataId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
