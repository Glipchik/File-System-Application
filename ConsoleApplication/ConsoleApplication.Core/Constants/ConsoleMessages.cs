namespace ConsoleApplication.Core.Constants
{
    public static class ConsoleMessages
    {
        public const string DirectoryNotFoundMessage = "Directory wasn't found.";
        public const string InvalidCommandMessage = "Invalid command";
        public const string InvalidFileNameMessage = "Invalid file name";
        public const string IncorrectPasswordMessage = "Failed password attempt";
        public const string IncorrectLoginMessage = "Failed login attempt";
        public const string InvalidLoginOrPasswordMessage = "Invalid login or password";
        public const string FileNotFoundMessage = "File wasn't found";
        public const string NoAccessToFileMessage = "No access to file";
        public const string InvalidSourceFileNameMessage = "There is no file with such source name";
        public const string StorageSizeExceedMessage = "Storage size exceeded";
        public const string InvalidFileSizeMessage = "File size is bigger than 50 Mb";
        public const string ValidLoggingInMessage = "You are in the system. Please, write a command";
        public const string InvalidLoginFileMessage = "Login file is empty. Please, fill it in";
        public const string FileAlreadyExistsMessage = "File with such name already exists";
        public const string UoWIsNullMessage = "Unit of work service is null";
        public const string CommandHelperIsNullMessage = "Command helper service is null";
        public const string StorageSectionIsNullMessage = "Storage section is null";
        public const string StoragePathIsNullMessage = "Storage path is null";
        public const string ProjectDirectoryOrItsParentsAreNullMessage = "Project directory or its parents are null";
        public const string WritingDirectlyToDiskMessage = "You are writing directly to disk, try other path";
    }
}
