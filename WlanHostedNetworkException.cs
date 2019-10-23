using System;

namespace WiFiDirectLegacyAPCSharp
{
    internal class WlanHostedNetworkException : Exception
    {
      
        public WlanHostedNetworkException(string message)
            : base(message)
        {

        }
      
        public WlanHostedNetworkException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
