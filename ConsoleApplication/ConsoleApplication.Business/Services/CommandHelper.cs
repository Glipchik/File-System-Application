using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Enums;
using ConsoleApplication.Core.Exceptions;

namespace ConsoleApplication.Business.Services
{
    public class CommandHelper : ICommandHelper
    {
        private readonly ILoginService _loginService;
        private readonly IUnitOfWork _unitOfWork;

        public CommandHelper(ILoginService loginService, IUnitOfWork unitOfWork)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void LaunchConsoleHandler(string storagePath)
        {
            bool loggedIn = false;

            Console.WriteLine("Please, enter login(--l {login}) and then password(--p {password})");

            while (!loggedIn)
            {
                Console.Write("> ");
                string? command = Console.ReadLine();

                if (string.IsNullOrEmpty(command))
                {
                    continue;
                }

                if (command.Equals(Command.exit.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                command = System.Text.RegularExpressions.Regex.Replace(command, @"\s+", " ");
                command = command.Trim();

                loggedIn = CallLogIn(ref command);
            }

            Console.WriteLine(ConsoleMessages.ValidLoggingInMessage);

            DateTime creationDate = _unitOfWork.GetCreationDate();

            bool exit = false;

            while (!exit)
            {
                bool anyCommandWorked = false;

                Console.Write("> ");
                string? operation = Console.ReadLine();

                if (string.IsNullOrEmpty(operation))
                {
                    Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
                    continue;
                }

                if (operation.Equals(Command.exit.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                operation = System.Text.RegularExpressions.Regex.Replace(operation, @"\s+", " ");
                string[] commandArr = operation.Trim().Split();

                if (commandArr.Length < 2)
                {
                    Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
                    continue;
                }

                if (commandArr[0].Equals(Command.user.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    && commandArr[1].Equals("info", StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = GetUserInfo(ref storagePath, ref creationDate);
                }

                if (anyCommandWorked)
                {
                    continue;
                }

                if (!commandArr[0].Equals(Command.file.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
                    continue;
                }

                if (commandArr[1].Equals(FileSubCommand.move.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = CallRenameFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.info.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = CallGetFileInfo(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.upload.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = CallUploadFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.remove.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = CallRemoveFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.download.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = CallDownloadFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.export.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = CallExportFile(ref commandArr);
                }

                if (!anyCommandWorked) Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
            }
        }

        private bool CallLogIn(ref string command)
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

        private bool GetUserInfo(ref string storagePath, ref DateTime creationDate)
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

        private bool CallRenameFile(ref string[] commandArr)
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

        private bool CallGetFileInfo(ref string[] commandArr)
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

        private bool CallUploadFile(ref string[] commandArr)
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

        private bool CallRemoveFile(ref string[] commandArr)
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

        private bool CallDownloadFile(ref string[] commandArr)
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

        private bool CallExportFile(ref string[] commandArr)
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
                        if (commandArr[2].LastIndexOf('\\') <= 2 && commandArr[2].LastIndexOf('/') <= 2)
                        {
                            throw new RecordDirectlyToDiskException(ConsoleMessages.WritingDirectlyToDiskMessage);
                        }

                        if (string.IsNullOrEmpty(commandArr[2].Replace("\"", "")))
                        {
                            Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                            return false;
                        }

                        string path = Path.GetFullPath(commandArr[2].Replace("\"", ""));

                        if (!path[^4..].Equals(FileFormat.json.ToString()))
                        {
                            Console.WriteLine(ConsoleMessages.InvalidFileNameMessage);
                            return false;
                        }

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