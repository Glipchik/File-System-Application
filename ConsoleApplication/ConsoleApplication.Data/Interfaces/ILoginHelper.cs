namespace ConsoleApplication.Data.Interfaces
{
    public interface ILoginHelper
    {
        public string GetLogin();

        public string GetPassword();

        public bool LogIn(string login);
    }
}
