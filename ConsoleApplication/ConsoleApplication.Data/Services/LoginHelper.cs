using ConsoleApplication.Core.Constants;
using ConsoleApplication.Core.Exceptions;
using ConsoleApplication.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ConsoleApplication.Data.Services
{
    public class LoginHelper : ILoginHelper
    {
        private readonly IConfigurationSection _loginPasswordSection;
        private string? _login;

        public LoginHelper(IConfigurationSection loginPasswordSection)
        {
            _loginPasswordSection = loginPasswordSection ?? throw new ArgumentNullException(nameof(loginPasswordSection), 
                ConsoleMessages.InvalidLoginFileMessage);
        }

        public bool LogIn(string login)
        {
            Console.Write("> ");
            string? password = Console.ReadLine();
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            password = System.Text.RegularExpressions.Regex.Replace(password, @"\s+", " ");
            password = password.Trim();

            if (string.IsNullOrEmpty(password) || password.Length < 5)
            {
                Console.WriteLine(ConsoleMessages.IncorrectPasswordMessage);
                return false;
            }

            if (!password[..4].Equals("--p "))
            {
                return false;
            }

            password = password[4..];

            if (_loginPasswordSection["Login"].Equals(login) && _loginPasswordSection["Password"].Equals(password))
            {
                _login = login;
                return true;
            }

            return false;
        }

        public string GetLogin()
        {
            return _login ?? throw new LoginNullException(ConsoleMessages.InvalidLoginFileMessage);
        }

        public string GetPassword() => _loginPasswordSection["Password"];
    }
}
