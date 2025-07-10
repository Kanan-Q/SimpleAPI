using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAPI.Core.Entities;

namespace SimpleAPI.DataAccess.Configuration;
public sealed class InformationConfiguration : IEntityTypeConfiguration<Information>
{
    public void Configure(EntityTypeBuilder<Information> entity)
    {
        entity.HasOne(x => x.Category).WithMany(x => x.Informations).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        entity.Property(x=>x.ProductName).HasMaxLength(50).IsRequired(true).IsUnicode(false);
        entity.Property(x=>x.Description).HasMaxLength(50).IsRequired(true).IsUnicode(false);
    }
}
