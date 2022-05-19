using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class FileStorageSectionNullException : ArgumentException
    {
        public FileStorageSectionNullException(string message) : base(message) { }

        protected FileStorageSectionNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
