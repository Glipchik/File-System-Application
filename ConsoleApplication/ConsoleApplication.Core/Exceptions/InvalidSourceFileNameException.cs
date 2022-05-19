using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class InvalidSourceFileNameException : ArgumentException
    {
        public InvalidSourceFileNameException(string message) : base(message) { }

        protected InvalidSourceFileNameException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
