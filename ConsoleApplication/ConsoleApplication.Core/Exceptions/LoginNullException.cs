using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class LoginNullException : ArgumentException
    {
        public LoginNullException(string message) : base(message) { }

        protected LoginNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}