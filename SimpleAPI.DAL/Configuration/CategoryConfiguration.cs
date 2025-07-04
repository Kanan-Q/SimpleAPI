using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.DataAccess.Configuration
{
    public sealed class CategoryConfiguration:IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasMany(x=>x.Informations).WithOne(x=>x.Category).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            entity.Property(x=>x.CategoryName).IsRequired(false).IsUnicode(false).HasMaxLength(40);
        }
    }
}
