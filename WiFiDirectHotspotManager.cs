
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.WiFiDirect;

namespace WiFiDirectLegacyAPCSharp
{
    internal class WiFiDirectHotspotManager
    {
        private IWlanHostedNetworkListener listener_;
        private WiFiDirectAdvertisementPublisher publisher_;
        private WiFiDirectAdvertisement advertisement_;
        private WiFiDirectLegacySettings legacySettings_;
        private WiFiDirectConnectionListener connectionListener_;
        private readonly List<WiFiDirectDevice> connectedDevices_;
        private string ssid_;
        private string passphrase_;

        public WiFiDirectHotspotManager(string ssid, string passphrase) : this()
        {
            Ssid = ssid;
            Passphrase = passphrase;
        }

        public WiFiDirectHotspotManager()
        {
            SsidProvided = false;
            PassphraseProvided = false;
            listener_ = null;

            connectedDevices_ = new List<WiFiDirectDevice>();
        }

        ~WiFiDirectHotspotManager()
        {
            publisher_?.Stop();
            Reset();
        }

        public List<WiFiDirectDevice> ConnectedDevices => connectedDevices_;
        public string Ssid
        {
            get => ssid_;
            set
            {
                ssid_ = value;
                SsidProvided = true;
            } 
        }
        public bool SsidProvided { get; set; }

        public string Passphrase
        {
            get => passphrase_;
            set
            {
                passphrase_ = value;
                PassphraseProvided = true;
            } 
        }

        public bool PassphraseProvided { get; set; }


        public void StartListener()
        {
            connectionListener_ = new WiFiDirectConnectionListener();
            connectionListener_.ConnectionRequested += ConnectionListenerOnConnectionRequested;
        }

        private async void ConnectionListenerOnConnectionRequested(WiFiDirectConnectionListener sender,
            WiFiDirectConnectionRequestedEventArgs args)
        {
            try
            {
                var connectionRequest = args.GetConnectionRequest();

                if (connectionRequest == null)
                {
                   throw new WlanHostedNetworkException(
                        "Call to ConnectionRequestedEventArgs.GetConnectionRequest() return a null result.");
                }

                var deviceInfo = connectionRequest.DeviceInformation;

                var wiFiDirectDevice = await WiFiDirectDevice.FromIdAsync(deviceInfo.Id);
                
                if (wiFiDirectDevice == null)
                {
                    throw new WlanHostedNetworkException($"Connection to {deviceInfo.Id} failed;");
                }

                if (wiFiDirectDevice.ConnectionStatus == WiFiDirectConnectionStatus.Connected)
                {
                    var endpointPairs = wiFiDirectDevice.GetConnectionEndpointPairs();
                    var connection = endpointPairs.First();
                    var remoteHostName = connection.RemoteHostName;
                    var remoteHostNameDisplay = remoteHostName.DisplayName;
                    listener_.OnDeviceConnected(remoteHostNameDisplay);
                    connectedDevices_.Add(wiFiDirectDevice);
                }
                wiFiDirectDevice.ConnectionStatusChanged += WfdDeviceOnConnectionStatusChanged;
            }catch(Exception ex)
            {
                throw new WlanHostedNetworkException(ex.Message, ex);
            }
        }


        private void WfdDeviceOnConnectionStatusChanged(WiFiDirectDevice wiFiDirectDevice, object args)
        {
            var endpointPairs = wiFiDirectDevice.GetConnectionEndpointPairs();
            var connection = endpointPairs.First();
            var remoteHostName = connection.RemoteHostName;
            var remoteHostNameDisplay = remoteHostName.DisplayName;
            var status = wiFiDirectDevice.ConnectionStatus;
            if (status == WiFiDirectConnectionStatus.Connected)
            {
                
                listener_.OnDeviceConnected(remoteHostNameDisplay);
            }
            else
            {
                if (connectedDevices_.Contains(wiFiDirectDevice))
                {
                    connectedDevices_.Remove(wiFiDirectDevice);
                }
                listener_.LogMessage($"{remoteHostNameDisplay} disconnected.");
            }
        }


        public void Start()
        {
            Reset();

            publisher_ = new WiFiDirectAdvertisementPublisher();
            publisher_.StatusChanged += PublisherOnStatusChanged;

            advertisement_ = publisher_.Advertisement;
            advertisement_.IsAutonomousGroupOwnerEnabled = true;

            legacySettings_ = advertisement_.LegacySettings;
            legacySettings_.IsEnabled = true;

            if (SsidProvided)
            {
                legacySettings_.Ssid = Ssid;
            }
            else
            {
                ssid_ = legacySettings_.Ssid;
            }

            var passwordCredentials = legacySettings_.Passphrase;
            if (PassphraseProvided)
            {
                passwordCredentials.Password = Passphrase;
            }
            else
            {
                passphrase_ = passwordCredentials.Password;
            }

            publisher_.Start();
        }

        public void Stop()
        {
            publisher_?.Stop();
        }

        private void PublisherOnStatusChanged(WiFiDirectAdvertisementPublisher sender, WiFiDirectAdvertisementPublisherStatusChangedEventArgs args)
        {
            try
            {
                var status = args.Status;

                switch (status)
                {
                    case WiFiDirectAdvertisementPublisherStatus.Started:
                        {
                            // Begin listening for connections and notify listener that the advertisement started
                            StartListener();
                            listener_?.OnAdvertisementStarted();
                            break;
                        }
                    case WiFiDirectAdvertisementPublisherStatus.Aborted:
                        {
                            // Check error and notify listener that the advertisement stopped
                            var error = args.Error;
                           
                            if (listener_ != null)
                            {
                                string message;

                                switch (error)
                                {
                                    case WiFiDirectError.RadioNotAvailable:
                                        message = "Advertisement aborted, Wi-Fi radio is turned off";
                                        break;

                                    case WiFiDirectError.ResourceInUse:
                                        message = "Advertisement aborted, Resource In Use";
                                        break;
                                    
                                    default:
                                        message = "Advertisement aborted, unknown reason";
                                        break;
                                }

                                listener_.OnAdvertisementAborted(message);
                            }
                            break;
                        }
                    case WiFiDirectAdvertisementPublisherStatus.Stopped:
                        {
                            // Notify listener that the advertisement is stopped
                            listener_?.OnAdvertisementStopped("Advertisement stopped");
                            break;
                        }
                }
            }
            catch (WlanHostedNetworkException ex)
            {
                listener_?.OnAsyncException(ex.Message);
            }
        }


        private void Reset()
        {
            if (connectionListener_ != null)
            {
                connectionListener_.ConnectionRequested -= ConnectionListenerOnConnectionRequested;
            }

            if (publisher_!= null)
            {
                publisher_.StatusChanged -= PublisherOnStatusChanged;
            }

            legacySettings_ = null;
            advertisement_ = null;
            publisher_ = null;

            if (connectionListener_ != null)
            {
                connectionListener_.ConnectionRequested += ConnectionListenerOnConnectionRequested;
                connectionListener_ = null;
            }

            connectedDevices_.Clear();
        }

        public void RegisterListener(IWlanHostedNetworkListener listener)
        {
            listener_ = listener;
        }
    }
}
