using System;

namespace WiFiDirectLegacyAPCSharp
{
    public class SimpleConsole : IWlanHostedNetworkListener
    {
        private readonly WiFiDirectHotspotManager wiFiDirectHotspotManager_;

        public SimpleConsole()
        {
            //wiFiDirectHotspotManager_ = new WiFiDirectHotspotManager("<your-ssid>, "<your-password>");
            wiFiDirectHotspotManager_ = new WiFiDirectHotspotManager();
            wiFiDirectHotspotManager_.RegisterListener(this);
        }

        private void ShowPrompt()
        {
            Console.WriteLine();
            Console.Write(">");
        }

        private void ShowHelp()
        {
            Console.WriteLine("Wi-Fi Direct Legacy AP Demo Usage:");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("start             : Start the legacy AP to accept connections");
            Console.WriteLine("stop              : Stop the legacy AP");
            Console.WriteLine("ssid <ssid>       : Configure the SSID before starting the legacy AP");
            Console.WriteLine("pass <passphrase> : Configure the passphrase before starting the legacy AP");
            Console.WriteLine("                  : Passphrase needs to be at least 8 characters");
            Console.WriteLine("quit|exit         : Exit");
            Console.WriteLine();
        }

        public bool ExecuteCommand(string command)
        {
            if (command == "quit" || command == "exit")
            {
                Console.WriteLine("Exiting");
                return false;
            }
            if (command == "start")
            {
                Console.WriteLine("Starting soft AP...");
                wiFiDirectHotspotManager_.Start();
                //WaitForSingleObjectEx(_apEvent.Get(), INFINITE, FALSE);
            }
            if (command == "stop")
            {
                Console.WriteLine("Stopping soft AP...");
                wiFiDirectHotspotManager_.Stop();
                //WaitForSingleObjectEx(_apEvent.Get(), INFINITE, FALSE);
            }
            if (command.Substring(0, 4).Equals("ssid"))
            {
                // Parse the SSID as the first non-space character after ssid
                var parts = command.Split(' ');
                if (parts.Length == 2)
                {
                    var ssid = parts[1];
                    Console.WriteLine($"Setting SSID to {ssid}");
                    wiFiDirectHotspotManager_.Ssid = ssid;
                }
                else
                {
                    Console.WriteLine("Setting SSID FAILED, bad input");
                }
            }
            if (command.Substring(0, 4).Equals("pass"))
            {
                // Parse the Passphrase as the first non-space character after pass
                var parts = command.Split(' ');
                if (parts.Length == 2)
                { 
                    var passphrase = parts[1];
                    Console.WriteLine($"Setting Passphrase to {passphrase}");
                    wiFiDirectHotspotManager_.Passphrase =passphrase;
                }
                else
                {
                    Console.WriteLine("Setting Passphrase FAILED, bad input");
                }
            }
            //else
            //{
            //    ShowHelp();
            //}

            return true;
        }
        public void RunConsole()
        {
            ShowHelp();
            ShowPrompt();
            bool running = true;
            while (running)
            {
                
                var command = Console.ReadLine();
                if (command.Length > 0)
                {
                    try
                    {
                        running = ExecuteCommand(command);
                    }
                    catch (WlanHostedNetworkException ex)
                    {
                        Console.WriteLine($"Caught Exception: {ex.Message}");
                    }
                }
                else
                {
                    ShowPrompt();
                }
            }
            

        }

        public void OnDeviceConnected(string remoteHostName)
        {
            Console.WriteLine();
            Console.WriteLine($"Peer connected: {remoteHostName}");
            ShowPrompt();
        }

        public void OnAdvertisementStarted()
        {
            Console.WriteLine();
            Console.WriteLine($"Soft AP started!");
            Console.WriteLine($"Peers can connect to: {wiFiDirectHotspotManager_.Ssid}");
            Console.WriteLine($"Passphrase: {wiFiDirectHotspotManager_.Passphrase}");
            ShowPrompt();
        }

        public void OnAdvertisementStopped(string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Soft AP stopped: {message}");
            ShowPrompt();
        }

        public void OnAdvertisementAborted(string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Soft AP aborted: {message}");
            ShowPrompt();
        }

        public void OnAsyncException(string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Caught exception in asynchronous method: {message}");
            ShowPrompt();
        }

        public void LogMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);

        }
    }
}
