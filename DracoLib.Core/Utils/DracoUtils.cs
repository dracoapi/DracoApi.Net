using System;

namespace DracoLib.Core.Utils
{
    internal class DracoUtils
    {
        internal static string GenerateDeviceId()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
    }
}