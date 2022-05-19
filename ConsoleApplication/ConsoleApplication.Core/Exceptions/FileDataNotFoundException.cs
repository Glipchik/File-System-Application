using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class FileDataNotFoundException : ArgumentException
    {
        public FileDataNotFoundException(string message) : base(message) { }

        protected FileDataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
