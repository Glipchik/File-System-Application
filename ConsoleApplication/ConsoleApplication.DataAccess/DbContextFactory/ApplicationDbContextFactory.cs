using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Exceptions;
using ConsoleApplication.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ConsoleApplication.DataAccess.DbContextFactory
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /*private readonly string _connectionString;
        public ApplicationDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }*/

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer(_connectionString);
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            if (currentDirectory?.Parent?.Parent is null)
            {
                throw new InvalidDirectoryException(ConsoleMessages.ProjectDirectoryOrItsParentsAreNullMessage);
            }

            string parentPath = currentDirectory.Parent.Parent.FullName;

            string pathToAppsettingsFile = Path.Combine(parentPath, Resources.ValidFullDirectoryPath).
                Replace(Resources.WrongFullDirectoryPath, Resources.ValidFullDirectoryPath);

            var connectionString = new ConfigurationBuilder().SetBasePath(pathToAppsettingsFile).
                   AddJsonFile("appsettings.json").Build().GetValue<string>("ConnectionString");

            return new ApplicationDbContext(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options);
        }
    }
}