using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Enums;
using ConsoleApplication.Core.Exceptions;

namespace ConsoleApplication.Business.Services
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ILoginService _loginService;
        private readonly IUnitOfWork _unitOfWork;

        public CommandExecutor(ILoginService loginService, IUnitOfWork unitOfWork)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool CallLogIn(ref string command)
        {
            string? operation = command;

            if (string.IsNullOrEmpty(operation) || operation.Length < 5)
            {
                Console.WriteLine(ConsoleMessages.IncorrectLoginMessage);
                return false;
            }

            string[] commandArr = operation.Trim().Split();

            if (commandArr.Length == 2 && commandArr[0].Equals("--l", StringComparison.InvariantCulture))
            {
                if (_loginService.LogIn(commandArr[1]))
                {
                    return true;
                }

                Console.WriteLine(ConsoleMessages.InvalidLoginOrPasswordMessage);
                return false;
            }

            Console.WriteLine(ConsoleMessages.IncorrectLoginMessage);
            return false;
        }

        public bool GetUserInfo(ref string storagePath, ref DateTime creationDate)
        {
            long size = 0;
            var directoryInfo = new DirectoryInfo(storagePath);

            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                size += fileInfo.Length;
            }

            Console.WriteLine($"login: {_loginService.GetLogin()}\ncreation Date: {creationDate.ToString("o")[..10]}\n" +
                $"storage used: {size} byte(s)");

            return true;
        }

        public bool CallRenameFile(ref string[] commandArr)
        {
            try
            {
                string sourceName = commandArr[2].Replace("\"", "");
                string destinationName = commandArr[3].Replace("\"", "");
                _unitOfWork.RenameFile(sourceName, destinationName);

                return true;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                return false;
            }
        }

        public bool CallGetFileInfo(ref string[] commandArr)
        {
            try
            {
                string fileName = commandArr[2].Replace("\"", "");
                _unitOfWork.GetFileInfo(fileName, _loginService.GetLogin());

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                return false;
            }

        }

        public bool CallUploadFile(ref string[] commandArr)
        {
            try
            {
                string path = commandArr[2].Replace("\"", "");
                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                    return false;
                }

                path = Path.GetFullPath(commandArr[2].Replace("\"", ""));
                if (File.Exists(path))
                {
                    _unitOfWork.UploadFile(path);

                    return true;
                }

                Console.WriteLine(ConsoleMessages.FileNotFoundMessage);

                return false;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                return false;
            }
        }

        public bool CallRemoveFile(ref string[] commandArr)
        {
            try
            {
                string fileName = commandArr[2].Replace("\"", "");
                _unitOfWork.DeleteFile(fileName);

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                return false;
            }
        }

        public bool CallDownloadFile(ref string[] commandArr)
        {
            try
            {
                string fileName = commandArr[2].Replace("\"", "");
                string folder = commandArr[3].Replace("\"", "");

                _unitOfWork.DownloadFile(fileName, folder);

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                return false;
            }
        }

        public bool CallExportFile(ref string[] commandArr)
        {
            try
            {
                if (commandArr.Length < 3)
                {
                    Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
                    return false;
                }

                if (commandArr[2].Equals("--info", StringComparison.InvariantCultureIgnoreCase))
                {
                    _unitOfWork.ExportData("", "info");
                }
                else if (commandArr.Length > 3 && commandArr[3].Equals("--format"))
                {
                    try
                    {
                        string path = Path.GetFullPath(commandArr[2].Replace("\"", ""));

                        if (commandArr[4].Equals(FileExportSubCommand.json.ToString(),
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            _unitOfWork.ExportData(path, FileFormat.json.ToString());
                        }
                        else if (commandArr[4].Equals(FileExportSubCommand.xml.
                            ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            _unitOfWork.ExportData(path, FileFormat.xml.ToString());
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine(ConsoleMessages.DirectoryNotFoundMessage);
                        return false;
                    }
                }
                else if (commandArr.Length == 3)
                {
                    try
                    {
                        if (commandArr[2].LastIndexOf('\\') <= 2 && commandArr[2].LastIndexOf('/') <= 2
                            && (commandArr[2].LastIndexOf('/') == commandArr[2].Length ||
                            commandArr[2].LastIndexOf('\\') == commandArr[2].Length))
                        {
                            throw new RecordDirectlyToDiskException(ConsoleMessages.WritingDirectlyToDiskMessage);
                        }

                        if (string.IsNullOrEmpty(commandArr[2].Replace("\"", "")))
                        {
                            Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                            return false;
                        }

                        string path = Path.GetFullPath(commandArr[2].Replace("\"", ""));

                        _unitOfWork.ExportData(path, FileFormat.json.ToString());
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine(ConsoleMessages.DirectoryNotFoundMessage);
                        return false;
                    }
                    catch (RecordDirectlyToDiskException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
                    return false;
                }

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
                return false;
            }
        }
    }
}