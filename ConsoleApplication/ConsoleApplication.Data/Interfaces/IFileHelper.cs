using ConsoleApplication.Domain.MetaData;

namespace ConsoleApplication.Data.Interfaces
{
    public interface IFileHelper
    {
        public DateTime GetCreationDate();

        public bool UploadFile(string path);

        public bool DeleteFile(string fileName);

        public bool RenameFile(string sourceName, string destinationName);

        public bool DownloadFile(string fileName, string directory);

        public void ExportData(string path, string format, List<FileData> serializableListWithDataForExporting);
    }
}
