using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class InvalidDirectoryException : ArgumentException
    {
        public InvalidDirectoryException(string message) : base(message) { }

        protected InvalidDirectoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
