using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Data.Configurations
{
    public class SupportIssueTypeConfiguration : IEntityTypeConfiguration<SupportIssueType>
    {
        public void Configure(EntityTypeBuilder<SupportIssueType> builder)
        {
            builder.ToTable("SupportIssueType");

            builder.HasIndex(x => x.PublicId, "IX_SupportIssueType_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();
        }
    }
}
