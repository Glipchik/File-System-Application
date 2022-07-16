using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Core.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.UI
{
    static class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var serviceProvider = startup.ConfigureServices().BuildServiceProvider();
            var commandHelper = serviceProvider.GetService<ICommandRecognizer>();

            if (commandHelper is null)
            {
                Console.WriteLine(ConsoleMessages.CommandHelperIsNullMessage);
                return;
            }

            string storagePath = startup.GetStoragePath();
            commandHelper.LaunchCommandHandling(storagePath);

            startup.DisposeServiceProvider(serviceProvider);
        }
    }
}