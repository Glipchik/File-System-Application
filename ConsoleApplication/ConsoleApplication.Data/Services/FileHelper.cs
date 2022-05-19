using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Enums;
using ConsoleApplication.Core.Exceptions;
using ConsoleApplication.Data.Interfaces;
using ConsoleApplication.Domain.MetaData;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApplication.Data.Services
{
    public class FileHelper : IFileHelper
    {
        private readonly string _storagePath;

        public FileHelper(string storagePath)
        {
            _storagePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
        }

        public DateTime GetCreationDate()
        {
            DateTime creationDate = Directory.GetCreationTime(Path.Combine(_storagePath));

            return creationDate;
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                if (File.Exists(Path.Combine(_storagePath, fileName)))
                {
                    File.Delete(Path.Combine(_storagePath, fileName));
                    return true;
                }
                Console.WriteLine(ConsoleMessages.FileNotFoundMessage);
                return false;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DownloadFile(string fileName, string directory)
        {
            try
            {
                directory = Path.GetFullPath(directory);

                if (File.Exists(Path.Combine(directory, fileName)))
                {
                    throw new FileAlreadyExistsException(ConsoleMessages.FileAlreadyExistsMessage);
                }

                File.Copy(Path.Combine(_storagePath, fileName), Path.Combine(directory, fileName));

                Console.WriteLine($"The file '{fileName}' has been downloaded");

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(ConsoleMessages.NoAccessToFileMessage);
                return false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(ConsoleMessages.FileNotFoundMessage);
                return false;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(ConsoleMessages.DirectoryNotFoundMessage);
                return false;
            }
        }

        public void ExportData(string path, string format, List<FileData> serializableListWithDataForExporting)
        {
            try
            {
                if (format.Equals(FileFormat.json.ToString(), StringComparison.InvariantCulture))
                {
                    using (var stream = File.Open(path, FileMode.OpenOrCreate))
                    {
                        stream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(serializableListWithDataForExporting, Newtonsoft.Json.Formatting.Indented)));
                    }

                    Console.WriteLine($"The meta-information has been exported, path = '{path}'");
                }

                if (format.Equals(FileFormat.xml.ToString(), StringComparison.InvariantCulture))
                {
                    var xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.NewLineHandling = NewLineHandling.Entitize;
                    var xmlSerializer = new XmlSerializer(typeof(List<FileData>));

                    using (XmlWriter xmlWriter = XmlWriter.Create(path, xmlWriterSettings))
                    {
                        xmlSerializer.Serialize(xmlWriter, serializableListWithDataForExporting);
                    }
                    Console.WriteLine($"The meta-information has been exported, path = '{path}'");
                }

                if (format.Equals("info", StringComparison.InvariantCulture))
                {
                    Console.WriteLine($"- {FileFormat.json}\n- {FileFormat.xml}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(ConsoleMessages.NoAccessToFileMessage);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(ConsoleMessages.DirectoryNotFoundMessage);
            }
        }

        public bool RenameFile(string sourceName, string destinationName)
        {
            try
            {
                if (File.Exists(Path.Combine(_storagePath, sourceName)))
                {
                    if (!string.IsNullOrEmpty(destinationName))
                    {
                        string sourceFullPath = Path.Combine(_storagePath, sourceName);
                        string destinationFullPath = Path.Combine(_storagePath, destinationName);
                        File.Move(sourceFullPath, destinationFullPath);
                        return true;
                    }

                    Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                    return false;
                }

                throw new InvalidSourceFileNameException(ConsoleMessages.InvalidSourceFileNameMessage);
            }
            catch (InvalidSourceFileNameException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool UploadFile(string path)
        {
            var fileInfo = new FileInfo(path);
            long size = 0;
            var directoryInfo = new DirectoryInfo(_storagePath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (FileInfo file in fileInfos)
            {
                size += file.Length;
            }
            if (size + fileInfo.Length > ConfigValues.MaxStorageSize)
            {
                Console.WriteLine(ConsoleMessages.StorageSizeExceedMessage);
                return false;
            }

            if (fileInfo.Length < ConfigValues.MaxFileSize)
            {
                path = Path.GetFullPath(path);

                if (File.Exists(Path.Combine(_storagePath, Path.GetFileName(path))))
                {
                    Console.WriteLine(ConsoleMessages.FileAlreadyExistsMessage);
                    return false;
                }

                File.Copy(path, Path.Combine(_storagePath, Path.GetFileName(path)));
                return true;
            }

            Console.WriteLine(ConsoleMessages.InvalidFileSizeMessage);
            return false;
        }
    }
}