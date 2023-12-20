using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Collections.Generic;

namespace WeatherApp
{
    public partial class MoreDataWindow : Window
    {
        public MoreDataWindow(List<string> selectedDevices)
        {
            InitializeComponent();

           
            using (var main = new Main())
            {
                
                var data = main.device
                    .Where(device => selectedDevices.Contains(device.device_id))
                    .Join(
                        main.gateway,
                        device => device.gateway_id,
                        gateway => gateway.gateway_id,
                        (device, gateway) => new { Device = device, Gateway = gateway }
                    )
                    .ToList();

                
                if (data.Any())
                {
                    // Print information for each selected device along with its gateway
                    foreach (var info in data)
                    {
                        moreDataText.Text += $"Device ID: {info.Device.device_id}\n" +
                                         
                                          $"Battery Status: {(info.Device.battery_status ?? 0)}\n" +
                                          $"Battery Voltage: {info.Device.BatV ?? 0}\n" +
                                          $"Gateway Latitude: {info.Gateway.latitude}\n" +
                                          $"Gateway Longitude: {info.Gateway.longitude}\n" +
                                          $"Gateway Altitude: {info.Gateway.altitude ?? 0}\n" +
                                          $"Average RSSI: {info.Gateway.avg_rssi}\n" +
                                          $"Average SNR: {info.Gateway.avg_snr}\n" +
                                          $"Average Airtime: {info.Gateway.avg_airtime}\n\n";
                    }
                }
                else
                {
                    moreDataText.Text = "No selected devices found.";
                }
            }
        }
    }
}
