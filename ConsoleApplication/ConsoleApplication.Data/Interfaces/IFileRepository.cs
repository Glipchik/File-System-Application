using ConsoleApplication.Domain.MetaData;

namespace ConsoleApplication.Data.Interfaces
{
    public interface IFileRepository
    {
        public bool UploadFile(string path);

        public bool DeleteFile(string fileName);

        public bool RenameFile(string sourceName, string destinationName);

        public void GetFileInfo(string fileName, string login);

        public bool DownloadFile(string fileName, string directory);

        public void DecreaseDownloadsNumber(string fileName);

        public List<FileData>? ExportData();
    }
}
