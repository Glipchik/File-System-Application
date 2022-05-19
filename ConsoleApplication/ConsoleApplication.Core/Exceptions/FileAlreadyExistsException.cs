using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class FileAlreadyExistsException : ArgumentException
    {
        public FileAlreadyExistsException(string message) : base(message) { }

        protected FileAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
