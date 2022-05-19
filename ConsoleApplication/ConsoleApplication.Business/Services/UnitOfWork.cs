using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Data.Interfaces;
using ConsoleApplication.Domain.MetaData;

namespace ConsoleApplication.Business.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileHelper _fileHelper;

        public UnitOfWork(IFileRepository repository, IFileHelper helper)
        {
            _fileRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _fileHelper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public DateTime GetCreationDate() => _fileHelper.GetCreationDate();

        public void UploadFile(string path)
        {
            bool uploadedRealFile = _fileHelper.UploadFile(path);

            if (!uploadedRealFile)
            {
                return;
            }

            bool uploadedMetaFile = _fileRepository.UploadFile(path);

            if (!uploadedMetaFile)
            {
                _fileHelper.DeleteFile(path);
            }
        }

        public void DeleteFile(string fileName)
        {
            bool deletedRealFile = _fileHelper.DeleteFile(fileName);

            if (!deletedRealFile)
            {
                return;
            }

            bool deletedMetaFile = _fileRepository.DeleteFile(fileName);

            if (!deletedMetaFile)
            {
                _fileHelper.UploadFile(fileName);
            }
        }

        public void RenameFile(string sourceName, string destinationName)
        {
            bool renamedRealFile = _fileHelper.RenameFile(sourceName, destinationName);

            if (!renamedRealFile)
            {
                return;
            }

            bool renamedMetaFile = _fileRepository.RenameFile(sourceName, destinationName);

            if (!renamedMetaFile)
            {
                (sourceName, destinationName) = (destinationName, sourceName);
                _fileHelper.RenameFile(sourceName, destinationName);
            }
        }

        public void GetFileInfo(string fileName, string login)
        {
            _fileRepository.GetFileInfo(fileName, login);
        }

        public void DownloadFile(string fileName, string directory)
        {
            bool downloadedMetaFile = _fileRepository.DownloadFile(fileName, directory);

            if (!downloadedMetaFile)
            {
                return;
            }

            bool downloadedRealFile = _fileHelper.DownloadFile(fileName, directory);

            if (!downloadedRealFile)
            {
                _fileRepository.DecreaseDownloadsNumber(fileName);
            }
        }

        public void ExportData(string path, string format)
        {
            List<FileData>? fileDataForExporting = _fileRepository.ExportData();
            if (fileDataForExporting is null) return;

            _fileHelper.ExportData(path, format, fileDataForExporting);
        }
    }
}