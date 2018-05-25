using System;
using System.Runtime.Serialization;

namespace DracoLib.Core.Exceptions
{
    [Serializable]
    internal class DracoError : Exception
    {
        public DracoError()
        {
        }

        public DracoError(string message) : base(message)
        {
        }

        public DracoError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DracoError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}