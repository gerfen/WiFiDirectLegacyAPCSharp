# WiFiDirectLegacyAPCSharp

## A port of the WiFiDirectLegacyAPDemo to a C# UWP console app

This is a quick and dirty port of the <a href="https://github.com/microsoft/Windows-classic-samples/tree/master/Samples/WiFiDirectLegacyAP" target="_blank">WiFiDirectLegacyAPDemo</a> written in C++ to a C# UWP console app.  


Wi-Fi Direct Legacy Connection C# UWP WRL Demo
===========================================

This sample is a simple UWP console application that uses WRL to demonstrate the Wi-Fi Direct legacy AP WinRT API from a UWP application.

Developers of UWP applications can use this sample to see how to replace the deprecated WlanHostedNetwork* API's with the new WinRT API's. These API's let an application start a Wi-Fi Direct Group Owner (GO) that acts as an Access Point (AP). This allows devices that do not support Wi-Fi Direct to connect to the Windows device running this application and communicate over TCP/UDP. The API's allow the developer to optionally specify an SSID and passphrase, or use randomly generated ones.

The sample is organized up into the following files:

- **WiFiDirectHotspotManager.cs** : This contains the code that uses the WiFi Direct API. This part may be used as is or modified to fit your application needs.
- **SimpleConsole.cs** : This is a simple console using Console input to take command line input and start or stop the Wi-Fi Direct legacy AP. It implements the IWlanHostedNetworkListener to handle receiving messages from the API.
- **Program.cs** : Main entry point that starts the simple console.

**Note** 
- This sample requires Windows 10 to execute, as it uses new API's. It also requires a Wi-Fi Card and Driver that supports Wi-Fi Direct. These API's **do not** support cross-connectivity so clients connecting to this device will not be able to use it for Internet access.

**Note** 
- Passphrase must be at least 8 characters! Otherwise you will get a message saying "Advertisement aborted, unknown reason"

Related topics
--------------

[Wi-Fi Direct WinRT API](https://msdn.microsoft.com/en-us/library/windows.devices.wifidirect.aspx)

[WlanHostedNetwork* API (deprecated in Windows 10)](https://msdn.microsoft.com/en-us/library/windows/desktop/dd815243.aspx)

[More information on MSDN](https://msdn.microsoft.com/en-us/library/windows/hardware/mt244265(v=vs.85).aspx)

Operating system requirements
-----------------------------

**Client:** Windows 10

**Server:** Windows 10

**Phone:**  Windows 10

Build the sample
----------------

1. Start Microsoft Visual Studio 2017/2019 and select **File** \> **Open** \> **Project/Solution**.
2. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**. 

Run the sample
--------------

To run this sample after building it, press F5 (run with debugging enabled) or Ctrl-F5 (run without debugging enabled) from Visual Studio Express 2017/2019 for Windows 10 or later versions of Visual Studio and Windows (any SKU). (Or select the corresponding options from the Debug menu.)
