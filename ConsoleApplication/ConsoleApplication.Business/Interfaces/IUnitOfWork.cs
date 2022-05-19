namespace ConsoleApplication.Business.Interfaces
{
    public interface IUnitOfWork
    {
        public DateTime GetCreationDate();

        public void UploadFile(string path);

        public void DeleteFile(string fileName);

        public void RenameFile(string sourceName, string destinationName);

        public void GetFileInfo(string fileName, string login);

        public void DownloadFile(string fileName, string directory);

        public void ExportData(string path, string format);
    }
}
