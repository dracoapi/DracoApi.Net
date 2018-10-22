using System;
using System.Runtime.Serialization;

namespace DracoLib.Core.Exceptions
{
    [Serializable]
    public class GoogleLoginException : Exception
    {
        public GoogleLoginException()
        {
        }

        public GoogleLoginException(string message) : base(message)
        {
        }

        public GoogleLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GoogleLoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
