
using Microsoft.EntityFrameworkCore;
using StudentOffice.Entities;

namespace StudentOffice.DbCtx
{
    public class ClientDbContext : DbContext
    {
        private string ConnectionString;
        public ClientDbContext(string ctx)
        {
            ConnectionString = ctx;
        }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }


        public DbSet<Student> Clients { get; set; }
        public DbSet<Representant> Representant { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Faculty> Faculites { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<Course> Courses { get; set; }
    }
}
