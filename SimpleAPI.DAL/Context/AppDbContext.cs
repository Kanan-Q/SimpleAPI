using Microsoft.EntityFrameworkCore;

namespace SimpleAPI.DAL.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions opt):base(opt){}
    }
}
