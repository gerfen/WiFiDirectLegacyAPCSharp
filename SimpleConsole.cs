using System;

namespace WiFiDirectLegacyAPCSharp
{
    public class SimpleConsole 
    {
        private readonly WiFiDirectHotspotManager wiFiDirectHotspotManager_;

        public SimpleConsole()
        {
            // Use the constructor below to use your own SSIS and passphrase.
            //wiFiDirectHotspotManager_ = new WiFiDirectHotspotManager("<your-ssid>, "<your-password>");

            wiFiDirectHotspotManager_ = new WiFiDirectHotspotManager();
           
            wiFiDirectHotspotManager_.AdvertisementAborted += OnAdvertisementAborted;
            wiFiDirectHotspotManager_.AdvertisementStarted += OnAdvertisementStarted;
            wiFiDirectHotspotManager_.AdvertisementStopped += OnAdvertisementStopped;
            wiFiDirectHotspotManager_.AsyncException += OnAsyncException;
            wiFiDirectHotspotManager_.DeviceConnected += OnDeviceConnected;
            wiFiDirectHotspotManager_.DeviceDisconnected += OnDeviceDisconnected;
           
        }

        ~SimpleConsole()
        {
            wiFiDirectHotspotManager_.AdvertisementAborted -= OnAdvertisementAborted;
            wiFiDirectHotspotManager_.AdvertisementStarted -= OnAdvertisementStarted;
            wiFiDirectHotspotManager_.AdvertisementStopped -= OnAdvertisementStopped;
            wiFiDirectHotspotManager_.AsyncException -= OnAsyncException;
            wiFiDirectHotspotManager_.DeviceConnected -= OnDeviceConnected;
            wiFiDirectHotspotManager_.DeviceDisconnected -= OnDeviceDisconnected;
        }

        private void OnAdvertisementAborted(object sender, string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Soft AP aborted: {message}");
            ShowPrompt();
        }

        private void OnAdvertisementStarted(object sender, string message)
        {
            Console.WriteLine();
            Console.WriteLine("Soft AP started!");
            Console.WriteLine($"Peers can connect to: {wiFiDirectHotspotManager_.Ssid}");
            Console.WriteLine($"Passphrase: {wiFiDirectHotspotManager_.Passphrase}");
            ShowPrompt();
        }

        public void OnAdvertisementStopped(object sender, string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Soft AP stopped: {message}");
            ShowPrompt();
        }

        private void OnAsyncException(object sender, string message)
        {
            Console.WriteLine();
            Console.WriteLine($"Caught exception in asynchronous method: {message}");
            ShowPrompt();
        }

        private void OnDeviceConnected(object sender, DeviceEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine($"{e.Message}");
            ShowPrompt();
        }

        private void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine(e.Message);
            ShowPrompt();
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
            }
            if (command == "stop")
            {
                Console.WriteLine("Stopping soft AP...");
                wiFiDirectHotspotManager_.Stop();
            }
            if (command.Substring(0, 4).Equals("ssid"))
            {
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
                if (command?.Length > 0)
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
    }
}
