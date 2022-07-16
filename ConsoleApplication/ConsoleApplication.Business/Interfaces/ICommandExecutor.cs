namespace ConsoleApplication.Business.Interfaces
{
    public interface ICommandExecutor
    {
        public bool CallLogIn(ref string command);

        public bool GetUserInfo(ref string storagePath, ref DateTime creationDate);

        public bool CallRenameFile(ref string[] commandArr);

        public bool CallGetFileInfo(ref string[] commandArr);

        public bool CallUploadFile(ref string[] commandArr);

        public bool CallRemoveFile(ref string[] commandArr);

        public bool CallDownloadFile(ref string[] commandArr);

        public bool CallExportFile(ref string[] commandArr);
    }
}
