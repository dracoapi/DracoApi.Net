using System;

namespace DracoApi.Net
{
    internal class DracoUtils
    {
        internal static string GenerateDeviceId()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
    }
}