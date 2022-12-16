using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GainVocab.API.Data.Configurations
{
    public class SupportIssueConfiguration : IEntityTypeConfiguration<SupportIssue>
    {
        public void Configure(EntityTypeBuilder<SupportIssue> builder)
        {
            builder.ToTable("SupportIssue");

            builder.HasIndex(x => x.PublicId, "IX_SupportIssue_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();

            builder.HasOne(e => e.IssueType)
                   .WithMany(e => e.Issues)
                   .HasForeignKey(e => e.IssueTypeId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .HasConstraintName("FK_SupportIssue_IssueTypeId");

            builder.Navigation(b => b.IssueType)
                   .AutoInclude();
        }
    }
}
