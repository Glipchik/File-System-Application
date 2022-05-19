namespace ConsoleApplication.Business.Interfaces
{
    public interface ILoginService
    {
        public bool LogIn(string login);

        public string GetLogin();
    }
}
