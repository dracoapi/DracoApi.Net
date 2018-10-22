using System;
using System.Runtime.Serialization;

namespace DracoLib.Core.Exceptions
{
    [Serializable]
    public class FacebookLoginException : Exception
    {
        public FacebookLoginException()
        {
        }

        public FacebookLoginException(string message) : base(message)
        {
        }

        public FacebookLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FacebookLoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
