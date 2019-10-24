using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFiDirect;

namespace WiFiDirectLegacyAPCSharp
{
    public class DeviceEventArgs : EventArgs
    {
        public DeviceEventArgs(WiFiDirectDevice device, string message) 
        {
            Device = device;
            Message = message;
        }
        public WiFiDirectDevice Device { get; }

        public string Message { get; }
    }
}
