using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiFiDirectLegacyAPCSharp
{
    internal interface IWlanHostedNetworkListener
    {
        void OnDeviceConnected(string remoteHostName);
        void OnAdvertisementStarted();
        void OnAdvertisementStopped(string message);
        void OnAdvertisementAborted(string message);
        void OnAsyncException(string message);
        void LogMessage(string message);

    }
}
