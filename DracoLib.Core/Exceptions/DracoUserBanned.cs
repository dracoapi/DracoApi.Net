using System;
using System.Runtime.Serialization;

namespace DracoLib.Core.Exceptions
{
    [Serializable]
    public class DracoUserBanned : Exception
    {
        public DracoUserBanned()
        {
        }

        public DracoUserBanned(string message) : base(message)
        {
        }

        public DracoUserBanned(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DracoUserBanned(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}