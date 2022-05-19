using ConsoleApplication.DataAccess.EntityConfigurations;
using ConsoleApplication.Domain.MetaData;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplication.DataAccess.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<FileData>? Files { get; set; }
        string _connectionString;

        /*public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }*/
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
            //optionsBuilder.UseSqlServer()
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileDataConfiguration());
        }
    }
}