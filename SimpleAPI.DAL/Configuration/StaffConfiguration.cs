using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAPI.Core.Entities;

namespace SimpleAPI.DataAccess.Configuration
{
    public sealed class StaffConfiguration:IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> entity)
        {
            entity.HasOne(x => x.Department).WithMany(x=>x.Staffs).HasForeignKey(x=>x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            entity.Property(x=>x.Name).HasMaxLength(40).IsRequired(false).IsUnicode(false);
            entity.Property(x=>x.Surmame).HasMaxLength(40).IsRequired(false).IsUnicode(false);
        }
    }
}
