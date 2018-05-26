using System;

namespace DracoLib.Core.Utils
{
    public class DracoUtils
    {
        public static string GenerateDeviceId()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
    }
}