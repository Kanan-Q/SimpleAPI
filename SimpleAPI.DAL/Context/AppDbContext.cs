using Microsoft.EntityFrameworkCore;
using SimpleAPI.Core.Entities;
using SimpleAPI.Core.Entities.Common;

namespace SimpleAPI.DAL.Context;
public class AppDbContext:DbContext
{
    public DbSet<Information> Informations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Department> Departments { get; set; }
    public AppDbContext(DbContextOptions opt):base(opt){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
