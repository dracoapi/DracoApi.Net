using DracoProtos.Core;
using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FServiceError : FServiceErrorBase
    {
        public FServiceError()
        {
        }
        
        public FServiceError(string cause, params object[] args)
        {
            this.cause = cause;
            this.args = args;
        }
    }
}
