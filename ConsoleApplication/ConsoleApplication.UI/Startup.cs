using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Business.Services;
using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Exceptions;
using ConsoleApplication.Data.Interfaces;
using ConsoleApplication.Data.Services;
using ConsoleApplication.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.UI
{
    public class Startup
    {
        private string? _storagePath;

        public string GetStoragePath()
        {
            if (string.IsNullOrEmpty(_storagePath))
            {
                throw new StoragePathNullException(ConsoleMessages.StoragePathIsNullMessage);
            }

            return _storagePath;
        }

        public ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var loginPasswordSection = configurationBuilder.GetSection("User");
            var connectionString = configurationBuilder.GetValue<string>("ConnectionString");
            _storagePath = configurationBuilder.GetSection("FileStorage")["StoragePath"];

            services.AddSingleton<ILoginHelper>(new LoginHelper(loginPasswordSection));
            services.AddSingleton<ILoginService, LoginService>();
            string storagePath = GetStoragePath();
            services.AddSingleton<IFileRepository, FileRepository>();
            services.AddSingleton<IFileHelper>(_ => new FileHelper(storagePath));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ICommandHelper, CommandHelper>();
            //services.AddSqlServer(connectionStringSection["sqlConnection"]);
            //services.AddDbContext<ApplicationDbContext>(_ => new ApplicationDbContext(connectionString));
            //services.AddSingleton<DbContext>(_ = new ApplicationDbContext(_ = new DbContextOptions<ApplicationDbContext>(), connectionString));
            //services.AddSingleton(_ => new ApplicationDbContextFactory(connectionString));
            //services.AddDbContextFactory<ApplicationDbContext>(_ => new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()));
            services.AddDbContextFactory<ApplicationDbContext>(_ => new DbContextOptionsBuilder());
            return services;
        }

        public void DisposeServiceProvider(ServiceProvider serviceProvider)
        {
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}