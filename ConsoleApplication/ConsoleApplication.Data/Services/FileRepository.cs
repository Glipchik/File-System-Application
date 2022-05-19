using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Exceptions;
using ConsoleApplication.Data.Interfaces;
using ConsoleApplication.DataAccess.Context;
using ConsoleApplication.Domain.MetaData;

namespace ConsoleApplication.Data.Services
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public FileRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                if (_applicationDbContext.Files is null)
                {
                    return false;
                }

                FileData? fileToRemove = _applicationDbContext.Files.FirstOrDefault(f => f.Name.Equals(fileName));
                if (fileToRemove is null) throw new FileDataNotFoundException(ConsoleMessages.InvalidFileNameMessage);

                _applicationDbContext.Remove(fileToRemove);
                _applicationDbContext.SaveChanges();

                Console.WriteLine($"The file '{fileName}' has been deleted");
                return true;
            }
            catch (FileDataNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DownloadFile(string fileName, string directory)
        {
            try
            {
                if (_applicationDbContext.Files is null)
                {
                    return false;
                }

                FileData? fileToDownload = _applicationDbContext.Files.FirstOrDefault(f => f.Name.Equals(fileName));

                if (fileToDownload is null) throw new FileDataNotFoundException(ConsoleMessages.InvalidFileNameMessage);
                fileToDownload.IncreaseDownloadsNumber();

                _applicationDbContext.SaveChanges();

                return true;
            }
            catch (FileDataNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void GetFileInfo(string fileName, string login)
        {
            if (_applicationDbContext.Files is null)
            {
                return;
            }

            FileData? fileToGetInfo = _applicationDbContext.Files.FirstOrDefault(f => f.Name.Equals(fileName));

            if (fileToGetInfo is null)
            {
                Console.WriteLine(ConsoleMessages.FileNotFoundMessage);
            }
            else
            {
                Console.WriteLine($"Login: {login}\nName: {fileToGetInfo.Name}\nExtension: " +
                    $"{fileToGetInfo.Extension}\nSize: {fileToGetInfo.Size} bytes\n" +
                    $"Creation Date: {fileToGetInfo.CreationDate.ToString("o")[..10]}\nDownloads number: " +
                    $"{fileToGetInfo.DownloadsNumber}\nLast write date: {fileToGetInfo.LastWriteTime.ToString("o")[..10]}");
            }
        }

        public bool RenameFile(string sourceName, string destinationName)
        {
            if (_applicationDbContext.Files is null)
            {
                Console.WriteLine(ConsoleMessages.FileNotFoundMessage);
                return false;
            }

            try
            {
                FileData? fileToRename = _applicationDbContext.Files.FirstOrDefault(f => f.Name.Equals(sourceName));

                if (fileToRename is null) throw new FileDataNotFoundException(ConsoleMessages.InvalidFileNameMessage);
                fileToRename.RenameFileData(destinationName);

                _applicationDbContext.SaveChanges();

                Console.WriteLine($"The file \"{sourceName}\" has been moved to '{destinationName}'");
                return true;
            }
            catch (FileDataNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool UploadFile(string path)
        {
            try
            {
                var fileInfo = new FileInfo(path);
                var addingFileData = new FileData(fileInfo.Name, path, fileInfo.Extension, fileInfo.Length,
                        fileInfo.CreationTime, fileInfo.LastWriteTime);
                _applicationDbContext.Add(addingFileData);
                _applicationDbContext.SaveChanges();

                Console.WriteLine($"The file '{path}' has been uploaded\n- file name: '{Path.GetFileName(path)}'" +
                    $"\n-file size: {fileInfo.Length} bytes\n- extension: '{fileInfo.Extension.Replace(".", "")}'");
                return true;
            }
            catch (ArgumentException)
            {
                Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                return false;
            }
        }

        public void DecreaseDownloadsNumber(string fileName)
        {
            if (_applicationDbContext.Files is null)
            {
                return;
            }

            FileData? fileToDownload = _applicationDbContext.Files.FirstOrDefault(f => f.Name.Equals(fileName));

            if (fileToDownload is null) return;

            fileToDownload.DecreaseDownloadsNumber();

            _applicationDbContext.SaveChanges();
        }

        public List<FileData>? ExportData()
        {
            return _applicationDbContext.Files?.ToList();
        }
    }
}