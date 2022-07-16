using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Enums;

namespace ConsoleApplication.Business.Services
{
    public class CommandRecognizer : ICommandRecognizer
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IUnitOfWork _unitOfWork;

        public CommandRecognizer(ICommandExecutor commandExecutor, IUnitOfWork unitOfWork)
        {
            _commandExecutor = commandExecutor ?? throw new ArgumentNullException(nameof(commandExecutor));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void LaunchCommandHandling(string storagePath)
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

                loggedIn = _commandExecutor.CallLogIn(ref command);
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
                    anyCommandWorked = _commandExecutor.GetUserInfo(ref storagePath, ref creationDate);
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
                    anyCommandWorked = _commandExecutor.CallRenameFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.info.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = _commandExecutor.CallGetFileInfo(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.upload.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = _commandExecutor.CallUploadFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.remove.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = _commandExecutor.CallRemoveFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.download.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = _commandExecutor.CallDownloadFile(ref commandArr);
                    if (!anyCommandWorked) continue;
                }

                if (commandArr[1].Equals(FileSubCommand.export.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    anyCommandWorked = _commandExecutor.CallExportFile(ref commandArr);
                }

                if (!anyCommandWorked) Console.WriteLine(ConsoleMessages.InvalidCommandMessage);
            }
        }
    }
}