using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GainVocab.API.Data.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Languages");

            builder.HasIndex(x => x.PublicId, "IX_Languages_PublicId")
                   .IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn();

            builder.Property(e => e.PublicId)
                   .HasMaxLength(36)
                   .HasDefaultValueSql("uuid_generate_v4()")
                   .IsFixedLength();
        }
    }
}
