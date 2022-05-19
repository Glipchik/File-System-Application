using ConsoleApplication.Business.Interfaces;
using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Exceptions;
using ConsoleApplication.Data.Interfaces;

namespace ConsoleApplication.Business.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginHelper _loginHelper;
        private string? _login;

        public LoginService(ILoginHelper loginHelper)
        {
            _loginHelper = loginHelper ?? throw new ArgumentNullException(nameof(loginHelper));
        }

        public string GetLogin()
        {
            if (string.IsNullOrEmpty(_login))
            {
                throw new LoginNullException(ConsoleMessages.InvalidLoginFileMessage);
            }

            return _login;
        }

        public bool LogIn(string login)
        {
            bool loggedIn = _loginHelper.LogIn(login);

            if (loggedIn)
            {
                _login = _loginHelper.GetLogin();
            }

            return loggedIn;
        }
    }
}