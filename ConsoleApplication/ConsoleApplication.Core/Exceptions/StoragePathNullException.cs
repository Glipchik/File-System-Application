using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class StoragePathNullException : ArgumentException
    {
        public StoragePathNullException(string message) : base(message) { }

        protected StoragePathNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
