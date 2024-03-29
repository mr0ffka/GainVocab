﻿using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Configurations
{
    public class CourseDataConfiguration : IEntityTypeConfiguration<CourseData>
    {
        public void Configure(EntityTypeBuilder<CourseData> builder)
        {
            builder.ToTable("CourseData");

            builder.HasIndex(x => x.PublicId, "IX_CourseData_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();

            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Data)
                   .HasForeignKey(c => c.CourseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_CourseData_CourseId");

            builder.HasMany(e => e.Examples)
                   .WithOne(e => e.CourseData)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Issues)
                   .WithOne(e => e.IssueEntity)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Progresses)
                   .WithOne(e => e.CurrentCourseData)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Navigation(b => b.Course)
                   .AutoInclude();
        }
    }
}
