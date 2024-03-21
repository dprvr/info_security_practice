using System;
using System.Runtime.Serialization;

namespace FC.Algo
{
    [Serializable]
    internal class MarkDecodingException : Exception
    {
        public MarkDecodingException()
        {

        }

        public MarkDecodingException(string message) : base(message)
        {
        }

        public MarkDecodingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MarkDecodingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}