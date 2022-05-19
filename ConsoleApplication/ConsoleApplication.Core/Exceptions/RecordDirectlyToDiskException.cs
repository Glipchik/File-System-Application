using System.Runtime.Serialization;

namespace ConsoleApplication.Core.Exceptions
{
    [Serializable]
    public class RecordDirectlyToDiskException : ArgumentException
    {
        public RecordDirectlyToDiskException(string message) : base(message) { }

        protected RecordDirectlyToDiskException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
