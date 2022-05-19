using ConsoleApplication.Core.Constants;

namespace ConsoleApplication.Domain.MetaData
{
    public class FileData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime CreationDate { get; set; }
        public int DownloadsNumber { get; private set; }
        public DateTime LastWriteTime { get; set; }

        public FileData() { }
        
        public FileData(string name, string path, string extension, long size, DateTime creationDate,
            DateTime lastWriteTime)
        {
            Name = name;
            Path = path;
            Extension = extension;
            Size = size;
            CreationDate = creationDate;
            DownloadsNumber = 0;
            LastWriteTime = lastWriteTime;
        }

        public void RenameFileData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name, ConsoleMessages.InvalidFileNameMessage);
            }

            Name = name;
        }

        public void IncreaseDownloadsNumber()
        {
            DownloadsNumber++;
        }

        public void DecreaseDownloadsNumber()
        {
            if (DownloadsNumber > 0)
            {
                DownloadsNumber--;
            }
        }
    }
}