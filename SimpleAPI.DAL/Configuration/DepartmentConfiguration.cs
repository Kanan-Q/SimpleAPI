using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAPI.Core.Entities;

namespace SimpleAPI.DataAccess.Configuration
{
    public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.HasMany(x => x.Staffs).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            entity.Property(x => x.DepartmentName).HasMaxLength(40).IsRequired(false).IsUnicode(false);
        }
    }
}
