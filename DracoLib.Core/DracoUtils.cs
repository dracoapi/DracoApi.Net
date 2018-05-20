using System;

namespace DracoLib.Core
{
    internal class DracoUtils
    {
        internal static string GenerateDeviceId()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
    }
}